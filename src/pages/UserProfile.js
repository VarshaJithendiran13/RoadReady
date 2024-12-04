import React, { useEffect, useState } from "react";
import { Box, Typography, TextField, Button, Card, CardContent, List, ListItem, Grid } from "@mui/material";
import { Edit, Save, Cancel, Delete } from "@mui/icons-material";
import { useNavigate } from "react-router-dom";

const UserProfile = () => {
  const [userInfo, setUserInfo] = useState(null);
  const [reservations, setReservations] = useState([]);
  const [carReservations, setCarReservations] = useState([]);
  const [editMode, setEditMode] = useState(false);
  const [updatedInfo, setUpdatedInfo] = useState({
    firstName: "",
    lastName: "",
    email: "",
    phoneNumber: "",
  });
  const [carId, setCarId] = useState("");
  const navigate = useNavigate();

  // Fetch personal info
  useEffect(() => {
    fetch("https://localhost:7173/api/User/profile", {
      headers: {
        Authorization: `Bearer ${localStorage.getItem("token")}`,
      },
    })
      .then((res) => res.json())
      .then((data) => {
        setUserInfo(data);
        setUpdatedInfo({
          firstName: data.firstName,
          lastName: data.lastName,
          email: data.email,
          phoneNumber: data.phoneNumber,
        });
      })
      .catch((err) => console.error("Error fetching user info:", err));
  }, []);

  // Fetch user's reservations
  useEffect(() => {
    fetch("https://localhost:7173/api/Reservation/user/reservations", {
      headers: {
        Authorization: `Bearer ${localStorage.getItem("token")}`,
      },
    })
      .then((res) => res.json())
      .then((data) => setReservations(data))
      .catch((err) => console.error("Error fetching reservations:", err));
  }, []);

  // Update user info
  const handleUpdate = () => {
    fetch("https://localhost:7173/api/User", {
      method: "PUT",
      headers: {
        "Content-Type": "application/json",
        Authorization: `Bearer ${localStorage.getItem("token")}`,
      },
      body: JSON.stringify(updatedInfo),
    })
      .then((res) => {
        if (res.ok) {
          alert("Profile updated successfully!");
          setEditMode(false);
          setUserInfo(updatedInfo);
        } else {
          alert("Failed to update profile.");
        }
      })
      .catch((err) => console.error("Error updating user info:", err));
  };

  // Fetch car reservations based on carId
  const handleFetchCarReservations = () => {
    if (!carId) {
      alert("Please enter a car ID.");
      return;
    }

    fetch(`https://localhost:7173/api/Reservation/car/${carId}`, {
      headers: {
        Authorization: `Bearer ${localStorage.getItem("token")}`,
      },
    })
      .then((res) => res.json())
      .then((data) => setCarReservations(data))
      .catch((err) => console.error("Error fetching car reservations:", err));
  };

  // Delete user account
  const handleDeleteAccount = () => {
    fetch("https://localhost:7173/api/User", {
      method: "DELETE",
      headers: {
        Authorization: `Bearer ${localStorage.getItem("token")}`,
      },
    })
      .then((res) => {
        if (res.ok) {
          alert("Account deleted successfully.");
          localStorage.removeItem("token");
          navigate("/login");
        } else {
          alert("Failed to delete account.");
        }
      })
      .catch((err) => console.error("Error deleting account:", err));
  };

  if (!userInfo) {
    return <Typography>Loading...</Typography>;
  }

  return (
    <Box
      sx={{
        display: "flex",
        justifyContent: "space-between",
        gap: "20px",
        padding: "40px",
        backgroundColor: "#1e1e2f",
        minHeight: "100vh",
      }}
    >
      {/* Left Section */}
      <Grid container direction="column" spacing={4} sx={{ flex: 1, maxWidth: "48%" }}>
        {/* User Info Section */}
        <Grid item>
          <Card
            sx={{
              backgroundColor: "#2c2c45",
              color: "#ffffff",
              boxShadow: "0px 4px 20px rgba(0, 0, 0, 0.4)",
              borderRadius: "15px",
              padding: "20px",
            }}
          >
            <CardContent>
              <Typography variant="h5" sx={{ marginBottom: 2, fontWeight: "bold", textAlign: "center" }}>
                User Details
              </Typography>
              {editMode ? (
                <>
                  <TextField
                    fullWidth
                    label="First Name"
                    value={updatedInfo.firstName || ""}
                    onChange={(e) => setUpdatedInfo({ ...updatedInfo, firstName: e.target.value })}
                    margin="normal"
                  />
                  <TextField
                    fullWidth
                    label="Last Name"
                    value={updatedInfo.lastName || ""}
                    onChange={(e) => setUpdatedInfo({ ...updatedInfo, lastName: e.target.value })}
                    margin="normal"
                  />
                  <TextField
                    fullWidth
                    label="Email"
                    value={updatedInfo.email || ""}
                    onChange={(e) => setUpdatedInfo({ ...updatedInfo, email: e.target.value })}
                    margin="normal"
                  />
                  <TextField
                    fullWidth
                    label="Phone Number"
                    value={updatedInfo.phoneNumber || ""}
                    onChange={(e) => setUpdatedInfo({ ...updatedInfo, phoneNumber: e.target.value })}
                    margin="normal"
                  />
                  <Box sx={{ display: "flex", justifyContent: "space-between", marginTop: 2 }}>
                    <Button startIcon={<Save />} variant="contained" color="success" onClick={handleUpdate}>
                      Save
                    </Button>
                    <Button startIcon={<Cancel />} variant="contained" color="error" onClick={() => setEditMode(false)}>
                      Cancel
                    </Button>
                  </Box>
                </>
              ) : (
                <>
                  <Typography><strong>First Name:</strong> {userInfo.firstName}</Typography>
                  <Typography><strong>Last Name:</strong> {userInfo.lastName}</Typography>
                  <Typography><strong>Email:</strong> {userInfo.email}</Typography>
                  <Typography><strong>Phone:</strong> {userInfo.phoneNumber}</Typography>
                  <Button startIcon={<Edit />} sx={{ marginTop: 2 }} onClick={() => setEditMode(true)} variant="contained">
                    Edit Profile
                  </Button>
                </>
              )}
              <Button
                startIcon={<Delete />}
                sx={{ marginTop: 2 }}
                color="error"
                variant="contained"
                onClick={handleDeleteAccount}
              >
                Delete Account
              </Button>
            </CardContent>
          </Card>
        </Grid>

        {/* View Reservations for Your Car Section */}
        <Grid item>
          <Card
            sx={{
              backgroundColor: "#2c2c45",
              color: "#ffffff",
              boxShadow: "0px 4px 20px rgba(0, 0, 0, 0.4)",
              borderRadius: "15px",
              padding: "20px",
            }}
          >
            <CardContent>
              <Typography variant="h5" sx={{ marginBottom: 2, fontWeight: "bold", textAlign: "center" }}>
                View Reservations for Your Car
              </Typography>
              <TextField
                fullWidth
                label="Enter Car ID"
                value={carId}
                onChange={(e) => setCarId(e.target.value)}
                margin="normal"
              />
              <Button
                sx={{ marginTop: 2 }}
                variant="contained"
                onClick={handleFetchCarReservations}
              >
                View Reservations
              </Button>

              <List sx={{ marginTop: 2 }}>
                {carReservations.length > 0 ? (
                  carReservations.map((reservation) => (
                    <ListItem
                      key={reservation.id}
                      sx={{
                        display: "flex",
                        justifyContent: "space-between",
                        alignItems: "center",
                        padding: "10px",
                        backgroundColor: "#3a3a55",
                        borderRadius: "10px",
                        marginBottom: "10px",
                      }}
                    >
                      <Box>
                        <Typography><strong>Car:</strong> {reservation.carId}</Typography>
                        <Typography><strong>Pickup:</strong> {reservation.pickupDate}</Typography>
                        <Typography><strong>Dropoff:</strong> {reservation.dropOffDate}</Typography>
                        <Typography><strong>Status:</strong> {reservation.reservationStatus}</Typography>
                      </Box>
                    </ListItem>
                  ))
                ) : (
                  <Typography>No reservations found for this car.</Typography>
                )}
              </List>
            </CardContent>
          </Card>
        </Grid>
      </Grid>

      {/* Right Section */}
      <Grid item sx={{ flex: 1, maxWidth: "48%" }}>
        <Card
          sx={{
            backgroundColor: "#2c2c45",
            color: "#ffffff",
            boxShadow: "0px 4px 20px rgba(0, 0, 0, 0.4)",
            borderRadius: "15px",
            padding: "20px",
            minHeight: "100%",
          }}
        >
          <CardContent>
            <Typography variant="h5" sx={{ marginBottom: 2, fontWeight: "bold", textAlign: "center" }}>
              Car Reservations
            </Typography>
            <List sx={{ marginTop: 2 }}>
              {reservations.length > 0 ? (
                reservations.map((reservation) => (
                  <ListItem
                    key={reservation.id}
                    sx={{
                      display: "flex",
                      justifyContent: "space-between",
                      alignItems: "center",
                      padding: "10px",
                      backgroundColor: "#3a3a55",
                      borderRadius: "10px",
                      marginBottom: "10px",
                    }}
                  >
                    <Box>
                      <Typography><strong>Car:</strong> {reservation.carId}</Typography>
                      <Typography><strong>Pickup:</strong> {reservation.pickupDate}</Typography>
                      <Typography><strong>Dropoff:</strong> {reservation.dropOffDate}</Typography>
                      <Typography><strong>Status:</strong> {reservation.reservationStatus}</Typography>
                    </Box>
                  </ListItem>
                ))
              ) : (
                <Typography>No reservations found.</Typography>
              )}
            </List>
          </CardContent>
        </Card>
      </Grid>
    </Box>
  );
};

export default UserProfile;
