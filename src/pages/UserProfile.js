import React, { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";

const UserProfile = () => {
  const [userInfo, setUserInfo] = useState(null);
  const [reservations, setReservations] = useState([]);
  const [editMode, setEditMode] = useState(false);
  const [updatedInfo, setUpdatedInfo] = useState({
    firstName: "",
    lastName: "",
    email: "",
    phoneNumber: "",
  });

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
          setUserInfo(updatedInfo); // Update the userInfo state with updatedInfo
        } else {
          alert("Failed to update profile.");
        }
      })
      .catch((err) => console.error("Error updating user info:", err));
  };

  // Cancel reservation
  const handleCancelReservation = (reservationId) => {
    fetch(`https://localhost:7173/api/Reservation/${reservationId}/cancel`, {
      method: "POST",
      headers: {
        Authorization: `Bearer ${localStorage.getItem("token")}`,
      },
    })
      .then((res) => {
        if (res.ok) {
          alert("Reservation canceled successfully!");
          setReservations(reservations.filter((r) => r.id !== reservationId));
        } else {
          alert("Failed to cancel reservation.");
        }
      })
      .catch((err) => console.error("Error canceling reservation:", err));
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
          navigate("/login"); // Redirect to login page after account deletion
        } else {
          alert("Failed to delete account.");
        }
      })
      .catch((err) => console.error("Error deleting account:", err));
  };

  if (!userInfo) {
    return <div>Loading...</div>; // Add a loading state when userInfo is null
  }

  return (
    <div style={styles.container}>
      <div style={styles.bgImage}>
        <div style={styles.overlay}></div>
        <div style={styles.content}>
          <h1 style={styles.title}>User Profile</h1>

          {/* Profile and Reservations Sections */}
          <div style={styles.profileContainer}>
            {/* Left Section: Edit Profile */}
            <div style={styles.profileSection}>
              <h2 style={styles.sectionTitle}>Edit Profile</h2>
              <div style={styles.card}>
                {editMode ? (
                  <div style={styles.form}>
                    <input
                      style={styles.input}
                      type="text"
                      value={updatedInfo.firstName || ""}
                      onChange={(e) =>
                        setUpdatedInfo({ ...updatedInfo, firstName: e.target.value })
                      }
                      placeholder="First Name"
                    />
                    <input
                      style={styles.input}
                      type="text"
                      value={updatedInfo.lastName || ""}
                      onChange={(e) =>
                        setUpdatedInfo({ ...updatedInfo, lastName: e.target.value })
                      }
                      placeholder="Last Name"
                    />
                    <input
                      style={styles.input}
                      type="email"
                      value={updatedInfo.email || ""}
                      onChange={(e) =>
                        setUpdatedInfo({ ...updatedInfo, email: e.target.value })
                      }
                      placeholder="Email"
                    />
                    <input
                      style={styles.input}
                      type="text"
                      value={updatedInfo.phoneNumber || ""}
                      onChange={(e) =>
                        setUpdatedInfo({ ...updatedInfo, phoneNumber: e.target.value })
                      }
                      placeholder="Phone Number"
                    />
                    <div>
                      <button style={styles.saveButton} onClick={handleUpdate}>
                        Save
                      </button>
                      <button
                        style={styles.cancelButton}
                        onClick={() => setEditMode(false)}
                      >
                        Cancel
                      </button>
                    </div>
                  </div>
                ) : (
                  <div>
                    <p><strong>First Name:</strong> {userInfo.firstName}</p>
                    <p><strong>Last Name:</strong> {userInfo.lastName}</p>
                    <p><strong>Email:</strong> {userInfo.email}</p>
                    <p><strong>Phone Number:</strong> {userInfo.phoneNumber}</p>
                    <button style={styles.editButton} onClick={() => setEditMode(true)}>
                      Edit Info
                    </button>
                  </div>
                )}
              </div>
            </div>

            {/* Right Section: Reservations */}
            <div style={styles.reservationSection}>
              <h2 style={styles.sectionTitle}>Your Reservations</h2>
              <div style={styles.card}>
              {reservations.length > 0 ? (
  <ul style={styles.reservationList}>
    {reservations.map((reservation) => (
      <li key={reservation.id} style={styles.reservationItem}>
        <p><strong>Car:</strong> {reservation.carName}</p>
        <p><strong>Pickup:</strong> {reservation.pickupDate}</p>
        <p><strong>Dropoff:</strong> {reservation.dropOffDate}</p>
        <p><strong>Status:</strong> {reservation.status}</p>
        {reservation.status === "Active" && (
          <button
            style={styles.cancelButton}
            onClick={() => handleCancelReservation(reservation.id)}
          >
            Cancel Reservation
          </button>
        )}
      </li>
    ))}
  </ul>
) : (
  <p>No reservations found.</p>
)}

              </div>
            </div>
          </div>

          {/* Delete Account Button */}
          <div style={styles.deleteAccountContainer}>
            <button style={styles.deleteAccountButton} onClick={handleDeleteAccount}>
              Delete Your Account
            </button>
          </div>
        </div>
      </div>
    </div>
  );
};

const styles = {
  container: {
    display: "flex",
    justifyContent: "center",
    alignItems: "center",
    minHeight: "100vh",
    position: "relative",
    backgroundColor: "#f4f6f9",
    fontFamily: "'Segoe UI', Tahoma, Geneva, Verdana, sans-serif",
  },

  content: {
    padding: "40px 20px",
    color: "#fff",
    textAlign: "center",
    maxWidth: "800px",
    margin: "0 auto",
  },
  title: {
    fontSize: "2.5rem",
    fontWeight: "bold",
    marginBottom: "20px",
  },
  profileContainer: {
    display: "flex",
    flexDirection: "column",
    alignItems: "center",
  },
  profileSection: {
    width: "100%",
    maxWidth: "600px",
    marginBottom: "30px",
  },
  reservationSection: {
    width: "100%",
    maxWidth: "600px",
  },
  card: {
    backgroundColor: "#ffffff",
    borderRadius: "8px",
    padding: "20px",
    boxShadow: "0 2px 10px rgba(0, 0, 0, 0.1)",
  },
  sectionTitle: {
    fontSize: "1.8rem",
    fontWeight: "bold",
    marginBottom: "20px",
  },
  input: {
    width: "100%",
    padding: "12px",
    marginBottom: "15px",
    borderRadius: "5px",
    fontSize: "1rem",
    border: "1px solid #ddd",
    backgroundColor: "#f9f9f9",
  },
  saveButton: {
    backgroundColor: "#28a745",
    color: "#fff",
    padding: "12px 20px",
    border: "none",
    borderRadius: "5px",
    cursor: "pointer",
  },
  cancelButton: {
    backgroundColor: "#dc3545",
    color: "#fff",
    padding: "12px 20px",
    border: "none",
    borderRadius: "5px",
    cursor: "pointer",
    marginLeft: "10px",
  },
  editButton: {
    backgroundColor: "#4CAF50",
    color: "#fff",
    padding: "12px 20px",
    border: "none",
    borderRadius: "5px",
    cursor: "pointer",
  },
  deleteAccountContainer: {
    marginTop: "20px",
  },
  deleteAccountButton: {
    backgroundColor: "#dc3545",
    color: "#fff",
    padding: "12px 20px",
    border: "none",
    borderRadius: "5px",
    cursor: "pointer",
  },
  reservationList: {
    listStyle: "none",
    padding: "0",
  },
  reservationItem: {
    padding: "12px",
    marginBottom: "15px",
    backgroundColor: "#fff",
    borderRadius: "8px",
    boxShadow: "0 2px 5px rgba(0, 0, 0, 0.1)",
    border: "1px solid #ddd",
  },
};

export default UserProfile;
