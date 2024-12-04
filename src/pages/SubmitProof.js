import React, { useState } from "react";
import { Box, Typography, Button } from "@mui/material";
import { useNavigate } from "react-router-dom"; // for navigation
import styled from "styled-components";

// Styled Components for Drag-and-Drop Animation
const DropArea = styled.div`
  border: 2px dashed #1976d2;
  border-radius: 8px;
  background-color: #f3f4f6;
  padding: 40px;
  text-align: center;
  cursor: pointer;
  transition: all 0.3s ease;

  &:hover {
    background-color: #e3f2fd;
  }

  ${(props) =>
    props.isDragging &&
    `
    border-color: #0d47a1;
    background-color: #bbdefb;
  `}
`;

const HighlightText = styled.span`
  color: #0d47a1;
  font-weight: bold;
`;

const SubmitProof = () => {
  const [file, setFile] = useState(null); // To store the file selected by the user
  const [isDragging, setIsDragging] = useState(false); // To handle drag state
  const navigate = useNavigate(); // Hook for navigation

  // Handle file drop
  const handleDrop = (e) => {
    e.preventDefault();
    e.stopPropagation();
    setIsDragging(false);

    // Only handle the first file for simplicity
    const droppedFile = e.dataTransfer.files[0];
    if (droppedFile) {
      setFile(droppedFile);
    }
  };

  // Handle drag events
  const handleDragOver = (e) => {
    e.preventDefault();
    e.stopPropagation();
    setIsDragging(true);
  };

  const handleDragLeave = (e) => {
    e.preventDefault();
    e.stopPropagation();
    setIsDragging(false);
  };

  // Handle file change for manual selection
  const handleFileChange = (e) => {
    setFile(e.target.files[0]);
  };

  // Handle form submission
  const handleSubmit = (e) => {
    e.preventDefault();

    // Perform validation to ensure a file is uploaded
    if (!file) {
      alert("Please upload a proof of your ID or driving license.");
      return;
    }

    // Simulate uploading the file (you can integrate with your backend API here)
    console.log("File uploaded:", file);

    // After successful upload, redirect to host-register page
    navigate("/host-register");
  };

  return (
    <Box
      sx={{
        display: "flex",
        flexDirection: "column",
        justifyContent: "center",
        alignItems: "center",
        padding: 4,
        minHeight: "100vh",
        backgroundColor: "#f5f5f5",
      }}
    >
      <Typography
        variant="h4"
        sx={{
          marginBottom: 3,
          fontWeight: "bold",
          textAlign: "center",
          color: "#0d47a1",
        }}
      >
        Kindly Submit Any Govt ID Proof To Become Host
      </Typography>

      <form
        onSubmit={handleSubmit}
        style={{
          width: "100%",
          maxWidth: "500px",
          textAlign: "center",
        }}
      >
        {/* Drag-and-Drop Area */}
        <DropArea
          onDragOver={handleDragOver}
          onDragLeave={handleDragLeave}
          onDrop={handleDrop}
          isDragging={isDragging}
        >
          <Typography variant="body1" sx={{ marginBottom: 1 }}>
            Drag and drop your file here, or click to select.
          </Typography>
          <Typography variant="caption">
            Accepted formats: <HighlightText>.jpg, .jpeg, .png, .pdf</HighlightText>
          </Typography>
          {file && (
            <Typography
              variant="subtitle1"
              sx={{
                marginTop: 2,
                fontWeight: "bold",
                color: "#0d47a1",
              }}
            >
              File Selected: {file.name}
            </Typography>
          )}
        </DropArea>

        {/* File Input Field */}
        <input
          type="file"
          accept=".jpg, .jpeg, .png, .pdf"
          onChange={handleFileChange}
          style={{
            display: "none", // Hide the default input field
          }}
          id="file-upload"
        />
        <label
          htmlFor="file-upload"
          style={{
            display: "inline-block",
            marginTop: "16px",
            cursor: "pointer",
            color: "#1976d2",
            textDecoration: "underline",
          }}
        >
          Or browse files from your computer
        </label>

        <Button
          type="submit"
          variant="contained"
          color="primary"
          fullWidth
          sx={{ marginTop: 3, padding: "10px 0" }}
        >
          Submit
        </Button>
      </form>
    </Box>
  );
};

export default SubmitProof;
