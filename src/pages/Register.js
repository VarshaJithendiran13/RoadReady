import React, { useState } from "react";
import { Box, TextField, Button, Typography } from "@mui/material";
import { motion } from "framer-motion";
import { toast } from "react-toastify";
import { useNavigate } from "react-router-dom"; // for navigation

// Importing Google Font (Roboto)
import '@fontsource/roboto'; // Use Roboto font globally

const Register = () => {
  const [firstName, setFirstName] = useState("");
  const [lastName, setLastName] = useState("");
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const [phoneNumber, setPhoneNumber] = useState("");
  const [role, setRole] = useState("user");
  
  const navigate = useNavigate(); // Hook to navigate to different routes

  const handleRegister = async (e) => {
    e.preventDefault();
    const user = { firstName, lastName, email, password, phoneNumber, role };

    const response = await fetch("https://localhost:7173/api/Auth/register", {
      method: "POST",
      headers: { "Content-Type": "application/json" },
      body: JSON.stringify(user),
    });

    if (response.ok) {
      toast.success("Registration successful!");
    } else {
      toast.error("Registration failed.");
    }
  };

  const handleRedirectAdminRegister = () => {
    navigate("/admin-register"); // Navigate to Admin registration page
  };

  const handleBecomeHost = () => {
    navigate("/host-register"); // Navigate to Host registration page
  };

  return (
    <Box
      sx={{
        minHeight: "100vh",
        position: "relative",
        display: "flex",
        justifyContent: "flex-start",
        alignItems: "flex-start",
        backgroundImage:
          "url('https://media.istockphoto.com/id/1419724017/photo/car-rental-agency-employee-giving-car-keys-to-beautiful-young-woman.jpg?b=1&s=612x612&w=0&k=20&c=Qd2P0VYuQGmethhCvWzR51PgZmGEN9YaWl47PiYL-3Q=')",
        backgroundSize: "cover",
        backgroundPosition: "center",
        backdropFilter: "blur(10px)",
      }}
    >
      {/* Overlay */}
      <Box
        sx={{
          position: "absolute",
          top: 0,
          left: 0,
          width: "100%",
          height: "100%",
          background: "rgba(0, 0, 0, 0.5)",
        }}
      />

      <Box
        component={motion.div}
        initial={{ opacity: 0, y: 30 }}
        animate={{ opacity: 1, y: 0 }}
        transition={{ duration: 0.7, ease: "easeOut" }}
        sx={{
          width: "100%",
          maxWidth: 380,
          padding: "20px",
          background: "rgba(255, 255, 255, 0.85)",
          borderRadius: "15px",
          boxShadow: "0 8px 20px rgba(0, 0, 0, 0.1)",
          textAlign: "center",
          zIndex: 2,
          height: "auto",
          position: "absolute",
          top: "10%",
          right: "5%",
        }}
      >
        <Typography
          variant="h6"
          sx={{
            fontWeight: 500,
            marginBottom: 2,
            color: "#37474f",
            letterSpacing: "0.5px",
            fontFamily: "'Roboto', sans-serif", // Use a different font
            fontSize: "1.2rem", // Smaller text size
          }}
        >
          Create an Account
        </Typography>
        <form onSubmit={handleRegister}>
          <TextField
            label="First Name"
            type="text"
            fullWidth
            required
            margin="normal"
            variant="outlined"
            value={firstName}
            onChange={(e) => setFirstName(e.target.value)}
            sx={{
              "& .MuiOutlinedInput-root": {
                "& fieldset": { border: "1px solid #e0e0e0" },
                "&:hover fieldset": { border: "1px solid #80cbc4" },
                "&.Mui-focused fieldset": { border: "1px solid #26a69a" },
              },
              background: "#f9f9f9",
              borderRadius: "8px",
              marginBottom: 1.5,
              "& .MuiInputBase-root": {
                height: "35px",
              },
            }}
          />
          <TextField
            label="Last Name"
            type="text"
            fullWidth
            required
            margin="normal"
            variant="outlined"
            value={lastName}
            onChange={(e) => setLastName(e.target.value)}
            sx={{
              "& .MuiOutlinedInput-root": {
                "& fieldset": { border: "1px solid #e0e0e0" },
                "&:hover fieldset": { border: "1px solid #80cbc4" },
                "&.Mui-focused fieldset": { border: "1px solid #26a69a" },
              },
              background: "#f9f9f9",
              borderRadius: "8px",
              marginBottom: 1.5,
              "& .MuiInputBase-root": {
                height: "35px",
              },
            }}
          />
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
              marginBottom: 1.5,
              "& .MuiInputBase-root": {
                height: "35px",
              },
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
              marginBottom: 1.5,
              "& .MuiInputBase-root": {
                height: "35px",
              },
            }}
          />
          <TextField
            label="Phone Number"
            type="tel"
            fullWidth
            required
            margin="normal"
            variant="outlined"
            value={phoneNumber}
            onChange={(e) => setPhoneNumber(e.target.value)}
            sx={{
              "& .MuiOutlinedInput-root": {
                "& fieldset": { border: "1px solid #e0e0e0" },
                "&:hover fieldset": { border: "1px solid #80cbc4" },
                "&.Mui-focused fieldset": { border: "1px solid #26a69a" },
              },
              background: "#f9f9f9",
              borderRadius: "8px",
              marginBottom: 1.5,
              "& .MuiInputBase-root": {
                height: "35px",
              },
            }}
          />

          {/* Role Select */}
          <Box sx={{ display: "flex", justifyContent: "space-between", alignItems: "center", marginBottom: 2 }}>
            <select
              value={role}
              onChange={(e) => setRole(e.target.value)}
              style={{
                padding: "8px",
                fontSize: "14px",
                borderRadius: "5px",
                backgroundColor: "#f9f9f9",
                border: "1px solid #ccc",
                width: "100%",
                marginBottom: "10px",
              }}
            >
              <option value="user">User</option>
              <option value="admin">Admin</option>
            </select>
          </Box>

          <Button
            type="submit"
            fullWidth
            sx={{
              background: "linear-gradient(to right, #26a69a, #80cbc4)",
              color: "white",
              padding: "10px",
              borderRadius: "8px",
              "&:hover": {
                background: "linear-gradient(to right, #80cbc4, #26a69a)",
              },
            }}
          >
            Register
          </Button>

          
        </form>
      </Box>
    </Box>
  );
};

export default Register;
