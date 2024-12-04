import React, { useState } from "react";
import { Box, TextField, Button, Typography, InputAdornment } from "@mui/material";
import { toast } from "react-toastify";
import DirectionsCarIcon from "@mui/icons-material/DirectionsCar";
import CalendarTodayIcon from "@mui/icons-material/CalendarToday";
import AttachMoneyIcon from "@mui/icons-material/AttachMoney";
import LocationOnIcon from "@mui/icons-material/LocationOn";
import ImageIcon from "@mui/icons-material/Image";

const HostRegister = () => {
  const [carDetails, setCarDetails] = useState({
    make: "",
    model: "",
    year: "",
    specifications: "",
    pricePerDay: 0,
    availabilityStatus: true,
    location: "",
    imageUrl: "",
  });
  const [carId, setCarId] = useState(null);

  const handleCarDetailsChange = (e) => {
    setCarDetails({
      ...carDetails,
      [e.target.name]: e.target.value,
    });
  };

  const handleAddCar = async (e) => {
    e.preventDefault();

    const response = await fetch("https://localhost:7173/api/Car", {
      method: "POST",
      headers: {
        "Content-Type": "application/json",
        Authorization: `Bearer ${localStorage.getItem("token")}`,
      },
      body: JSON.stringify(carDetails),
    });

    if (response.ok) {
      const car = await response.json();
      setCarId(car.id); // Save the car ID
      toast.success(`Car added successfully! Car ID: ${car.id}`);
    } else {
      toast.error("Failed to add car.");
    }
  };

  return (
    <Box
      sx={{
        backgroundColor: "#f5f5f5",
        padding: 4,
        borderRadius: 3,
        maxWidth: "600px",
        margin: "auto",
        boxShadow: "0px 4px 12px rgba(0, 0, 0, 0.2)",
        textAlign: "center",
      }}
    >
      <Typography
        variant="h4"
        sx={{
          marginBottom: 3,
          fontWeight: "bold",
          color: "#0d47a1",
        }}
      >
        Become a Host: Register Your Car
      </Typography>

      <form onSubmit={handleAddCar}>
        <TextField
          label="Car Make"
          name="make"
          fullWidth
          value={carDetails.make}
          onChange={handleCarDetailsChange}
          required
          sx={{ marginBottom: 2 }}
          InputProps={{
            startAdornment: (
              <InputAdornment position="start">
                <DirectionsCarIcon sx={{ color: "#0d47a1" }} />
              </InputAdornment>
            ),
          }}
        />
        <TextField
          label="Car Model"
          name="model"
          fullWidth
          value={carDetails.model}
          onChange={handleCarDetailsChange}
          required
          sx={{ marginBottom: 2 }}
          InputProps={{
            startAdornment: (
              <InputAdornment position="start">
                <DirectionsCarIcon sx={{ color: "#0d47a1" }} />
              </InputAdornment>
            ),
          }}
        />
        <TextField
          label="Car Year"
          name="year"
          type="number"
          fullWidth
          value={carDetails.year}
          onChange={handleCarDetailsChange}
          required
          sx={{ marginBottom: 2 }}
          InputProps={{
            startAdornment: (
              <InputAdornment position="start">
                <CalendarTodayIcon sx={{ color: "#0d47a1" }} />
              </InputAdornment>
            ),
          }}
        />
        <TextField
          label="Specifications"
          name="specifications"
          fullWidth
          value={carDetails.specifications}
          onChange={handleCarDetailsChange}
          required
          sx={{ marginBottom: 2 }}
        />
        <TextField
          label="Price per Day"
          name="pricePerDay"
          type="number"
          fullWidth
          value={carDetails.pricePerDay}
          onChange={handleCarDetailsChange}
          required
          sx={{ marginBottom: 2 }}
          InputProps={{
            startAdornment: (
              <InputAdornment position="start">
                <AttachMoneyIcon sx={{ color: "#0d47a1" }} />
              </InputAdornment>
            ),
          }}
        />
        <TextField
          label="Location"
          name="location"
          fullWidth
          value={carDetails.location}
          onChange={handleCarDetailsChange}
          required
          sx={{ marginBottom: 2 }}
          InputProps={{
            startAdornment: (
              <InputAdornment position="start">
                <LocationOnIcon sx={{ color: "#0d47a1" }} />
              </InputAdornment>
            ),
          }}
        />
        <TextField
          label="Car Image URL"
          name="imageUrl"
          fullWidth
          value={carDetails.imageUrl}
          onChange={handleCarDetailsChange}
          sx={{ marginBottom: 2 }}
          InputProps={{
            startAdornment: (
              <InputAdornment position="start">
                <ImageIcon sx={{ color: "#0d47a1" }} />
              </InputAdornment>
            ),
          }}
        />
        <Button
          type="submit"
          fullWidth
          variant="contained"
          color="primary"
          sx={{
            backgroundColor: "#0d47a1",
            padding: "10px",
            fontWeight: "bold",
            marginTop: 2,
            "&:hover": {
              backgroundColor: "#1565c0",
            },
          }}
        >
          Add Car
        </Button>
      </form>

      {/* Display Car ID */}
      {carId && (
        <Typography
          sx={{
            marginTop: 3,
            padding: 2,
            backgroundColor: "#e3f2fd",
            color: "#0d47a1",
            borderRadius: 2,
          }}
        >
          Car registered successfully! Your Car ID is: <strong>{carId}</strong>
        </Typography>
      )}
    </Box>
  );
};

export default HostRegister;
