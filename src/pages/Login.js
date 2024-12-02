import React, { useState, useContext } from "react";
import { useNavigate } from "react-router-dom";
import AuthContext from "../components/AuthContext";
import { login as loginService } from "../components/Services/AuthService";
import { Box, TextField, Button, Typography } from "@mui/material";
import { motion } from "framer-motion";
import { toast } from "react-toastify";

const Login = () => {
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const [isAdmin, setIsAdmin] = useState(false);
  const { login } = useContext(AuthContext);
  const navigate = useNavigate();

  const handleSubmit = async (e) => {
    e.preventDefault();
    const role = isAdmin ? "admin" : "user";
    try {
      const token = await loginService(email, password, role);
      login(token);
      toast.success("Login Successful");
      navigate("/roadreadyhome");
    } catch (error) {
      toast.error("Login failed: Invalid credentials or unexpected error");
    }
  };

  return (
    <Box
      sx={{
        minHeight: "100vh",
        display: "flex",
        justifyContent: "center",
        alignItems: "center",
        position: "relative",
        // Background image with blur effect applied directly
        backgroundImage: "url('https://img.freepik.com/free-photo/stylish-elegant-woman-car-salon_1157-20980.jpg?t=st=1732863843~exp=1732867443~hmac=06d0296e576820f3452cfd969318dd6bb949295c2c2aa74e7683c8dc2af65f39&w=996')",
        backgroundSize: "cover",
        backgroundPosition: "center",
      }}
    >
      {/* Add overlay for blur effect */}
      <Box
        sx={{
          position: "absolute",
          top: 0,
          left: 0,
          width: "100%",
          height: "100%",
          background: "rgba(0, 0, 0, 0.5)", // Semi-transparent dark overlay
          backdropFilter: "blur(10px)", // Apply blur effect
        }}
      />

      {/* Admin Toggle Button */}
      <Button
        onClick={() => setIsAdmin(!isAdmin)}
        sx={{
          position: "absolute",
          top: 20,
          right: 20,
          padding: "6px 18px",
          fontSize: "0.85rem",
          fontWeight: "bold",
          background: isAdmin ? "rgba(255, 183, 77, 0.15)" : "rgba(129, 230, 217, 0.15)",
          color: isAdmin ? "#f57c00" : "#26a69a",
          borderRadius: "25px",
          boxShadow: "0 3px 6px rgba(0, 0, 0, 0.1)",
          "&:hover": {
            background: isAdmin ? "#ffd180" : "#80cbc4",
          },
        }}
      >
        {isAdmin ? "Admin Mode" : "Login as Admin"}
      </Button>

      {/* Login Form */}
      <Box
        component={motion.div}
        initial={{ opacity: 0, y: 30 }}
        animate={{ opacity: 1, y: 0 }}
        transition={{ duration: 0.7, ease: "easeOut" }}
        sx={{
          width: "100%",
          maxWidth: 340,
          padding: "25px",
          background: "rgba(255, 255, 255, 0.8)", // Semi-transparent background for form
          borderRadius: "15px",
          boxShadow: "0 8px 20px rgba(0, 0, 0, 0.1)",
          textAlign: "center",
          zIndex: 2, // Make sure the form appears above the overlay
        }}
      >
        <Typography
          variant="h5"
          sx={{
            fontWeight: 600,
            marginBottom: 2,
            color: "#37474f",
            letterSpacing: "0.5px",
          }}
        >
          Welcome Back
        </Typography>
        <form onSubmit={handleSubmit}>
          <TextField
            label="Email"
            type="email"
            fullWidth
            required
            margin="normal"
            variant="outlined"
            value={email}
            onChange={(e) => setEmail(e.target.value)}
            sx={{
              "& .MuiOutlinedInput-root": {
                "& fieldset": { border: "1px solid #e0e0e0" },
                "&:hover fieldset": { border: "1px solid #80cbc4" },
                "&.Mui-focused fieldset": { border: "1px solid #26a69a" },
              },
              background: "#f9f9f9",
              borderRadius: "8px",
              marginBottom: 2,
            }}
          />
          <TextField
            label="Password"
            type="password"
            fullWidth
            required
            margin="normal"
            variant="outlined"
            value={password}
            onChange={(e) => setPassword(e.target.value)}
            sx={{
              "& .MuiOutlinedInput-root": {
                "& fieldset": { border: "1px solid #e0e0e0" },
                "&:hover fieldset": { border: "1px solid #80cbc4" },
                "&.Mui-focused fieldset": { border: "1px solid #26a69a" },
              },
              background: "#f9f9f9",
              borderRadius: "8px",
            }}
          />
          <Button
            type="submit"
            variant="contained"
            fullWidth
           //onClick={() => navigate("/roadreadyhome")}
            sx={{
              marginTop: 2,
              padding: "10px",
              fontSize: "0.95rem",
              fontWeight: "600",
              textTransform: "none",
              background: "linear-gradient(to right, #26a69a, #80cbc4)",
              color: "white",
              borderRadius: "8px",
              boxShadow: "0 3px 6px rgba(0, 0, 0, 0.2)",
              "&:hover": {
                background: "linear-gradient(to right, #80cbc4, #26a69a)",
                boxShadow: "0 5px 12px rgba(0, 0, 0, 0.2)",
              },
            }}
          >
            Login
          </Button>
          <Button
            variant="text"
            onClick={() => navigate("/forgot-password")}
            sx={{
              paddingRight: "20px",
              marginTop: 2,
              color: "#26a69a",
              textTransform: "none",
              fontWeight: "500",
              fontSize: "0.85rem",
              "&:hover": { color: "#004d40" },
            }}
          >
            Forgot Password?
          </Button>

          {/* New User Registration Button */}
          <Button
            variant="text"
            onClick={() => navigate("/register")}
            sx={{
              marginTop: 2,
              paddingLeft: "50px",  // Add padding to the left
              paddingRight: "0px",
              color: "#f44336", // Red color for registration
              textTransform: "none",
              fontWeight: "500",
              fontSize: "0.85rem",
              "&:hover": { color: "#c62828" },
            }}
          >
            New User? Register
          </Button>
        </form>
      </Box>
    </Box>
  );
};

export default Login;
