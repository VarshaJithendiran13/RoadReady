import React, { useState } from "react";
import {
  TextField,
  Button,
  Typography,
  Box,
  Container,
  InputAdornment,
  IconButton,
  LinearProgress,
} from "@mui/material";
import { motion } from "framer-motion";
import { toast } from "react-toastify";
import Visibility from "@mui/icons-material/Visibility";
import VisibilityOff from "@mui/icons-material/VisibilityOff";

const ForgotPassword = () => {
  const [email, setEmail] = useState("");
  const [resetToken, setResetToken] = useState("");
  const [newPassword, setNewPassword] = useState("");
  const [confirmPassword, setConfirmPassword] = useState("");
  const [showPassword, setShowPassword] = useState(false);
  const [passwordStrength, setPasswordStrength] = useState(0);
  const [step, setStep] = useState(1);

  const calculatePasswordStrength = (password) => {
    let strength = 0;
    if (password.length >= 8) strength += 1;
    if (/[A-Z]/.test(password)) strength += 1;
    if (/[0-9]/.test(password)) strength += 1;
    if (/[@$!%*?&#]/.test(password)) strength += 1;
    return (strength / 4) * 100;
  };

  const handlePasswordChange = (e) => {
    const password = e.target.value;
    setNewPassword(password);
    setPasswordStrength(calculatePasswordStrength(password));
  };

  const handleResetRequest = async (e) => {
    e.preventDefault();
    const response = await fetch("https://localhost:7173/api/Auth/forgot-password", {
      method: "POST",
      headers: { "Content-Type": "application/json" },
      body: JSON.stringify({ email }),
    });

    if (response.ok) {
      toast.success("Password reset email sent!");
      setStep(2);
    } else {
      toast.error("Failed to send reset email.");
    }
  };

  const handlePasswordReset = async (e) => {
    e.preventDefault();

    if (newPassword !== confirmPassword) {
      toast.error("Passwords do not match.");
      return;
    }

    const response = await fetch("https://localhost:7173/api/Auth/reset-password", {
      method: "POST",
      headers: { "Content-Type": "application/json" },
      body: JSON.stringify({ resetToken, newPassword, confirmPassword }),
    });

    if (response.ok) {
      toast.success("Password has been successfully reset!");
    } else {
      toast.error("Failed to reset password.");
    }
  };

  return (
    <Container maxWidth="sm">
      <Box
        component={motion.div}
        initial={{ opacity: 0, scale: 0.9 }}
        animate={{ opacity: 1, scale: 1 }}
        transition={{ duration: 0.8 }}
        sx={{
          padding: 4,
          borderRadius: "8px",
          boxShadow: "0px 10px 20px rgba(0, 0, 0, 0.1)",
          background: "#f4f7fc",
        }}
      >
        <Typography
          variant="h4"
          align="center"
          sx={{
            marginBottom: 3,
            fontWeight: "bold",
            color: "#2c3e50",
            textShadow: "1px 1px 2px rgba(0, 0, 0, 0.1)",
          }}
        >
          {step === 1 ? "Forgot Password" : "Reset Your Password"}
        </Typography>

        {/* Email Form */}
        {step === 1 && (
          <form onSubmit={handleResetRequest}>
            <TextField
              label="Enter Your Email"
              type="email"
              fullWidth
              required
              variant="outlined"
              value={email}
              onChange={(e) => setEmail(e.target.value)}
              sx={{ marginBottom: 2 }}
            />
            <Button
              type="submit"
              variant="contained"
              fullWidth
              sx={{
                background: "linear-gradient(135deg, #2575fc, #6a11cb)",
                color: "white",
                "&:hover": { background: "linear-gradient(135deg, #6a11cb, #2575fc)" },
              }}
            >
              Send Reset Link
            </Button>
          </form>
        )}

        {/* Token and Password Form */}
        {step === 2 && (
          <form onSubmit={handlePasswordReset}>
            <TextField
              label="Reset Token"
              type="text"
              fullWidth
              required
              variant="outlined"
              value={resetToken}
              onChange={(e) => setResetToken(e.target.value)}
              sx={{ marginBottom: 2 }}
            />

            <TextField
              label="New Password"
              type={showPassword ? "text" : "password"}
              fullWidth
              required
              variant="outlined"
              value={newPassword}
              onChange={handlePasswordChange}
              InputProps={{
                endAdornment: (
                  <InputAdornment position="end">
                    <IconButton onClick={() => setShowPassword(!showPassword)}>
                      {showPassword ? <VisibilityOff /> : <Visibility />}
                    </IconButton>
                  </InputAdornment>
                ),
              }}
              sx={{ marginBottom: 2 }}
            />
            <LinearProgress
              variant="determinate"
              value={passwordStrength}
              sx={{
                height: 8,
                borderRadius: 4,
                marginBottom: 2,
                "& .MuiLinearProgress-bar": {
                  backgroundColor:
                    passwordStrength < 50 ? "red" : passwordStrength < 75 ? "orange" : "green",
                },
              }}
            />

            <TextField
              label="Confirm Password"
              type={showPassword ? "text" : "password"}
              fullWidth
              required
              variant="outlined"
              value={confirmPassword}
              onChange={(e) => setConfirmPassword(e.target.value)}
              InputProps={{
                endAdornment: (
                  <InputAdornment position="end">
                    <IconButton onClick={() => setShowPassword(!showPassword)}>
                      {showPassword ? <VisibilityOff /> : <Visibility />}
                    </IconButton>
                  </InputAdornment>
                ),
              }}
              sx={{ marginBottom: 2 }}
            />

            <Button
              type="submit"
              variant="contained"
              fullWidth
              sx={{
                background: "linear-gradient(135deg, #2575fc, #6a11cb)",
                color: "white",
                "&:hover": { background: "linear-gradient(135deg, #6a11cb, #2575fc)" },
              }}
            >
              Reset Password
            </Button>
          </form>
        )}
      </Box>
    </Container>
  );
};

export default ForgotPassword;
