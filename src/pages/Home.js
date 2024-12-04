import React from "react";
import { Link } from "react-router-dom";

// Home Component with enhanced styling
const Home = () => (
  <div style={styles.container}>
    <div style={styles.overlay}>
      <h1 style={styles.title}>
        <span style={styles.highlight}>Welcome</span> to RoadReady 
      </h1>
      <p style={styles.subtitle}>
        Discover the perfect car for every journey. Affordable, reliable, and convenient.
      </p>
      <div style={styles.features}>
        <div style={styles.featureItem}>
          <i className="fas fa-car" style={styles.icon}></i>
          <p style={styles.featureText}>Wide Selection</p>
        </div>
        <div style={styles.featureItem}>
          <i className="fas fa-shield-alt" style={styles.icon}></i>
          <p style={styles.featureText}>Secure Rentals</p>
        </div>
        <div style={styles.featureItem}>
          <i className="fas fa-road" style={styles.icon}></i>
          <p style={styles.featureText}>Unlimited Mileage</p>
        </div>
      </div>
      <div style={styles.buttonContainer}>
        <Link to="/login" style={styles.button}>
          Login
        </Link>
        <Link to="/register" style={{ ...styles.button, backgroundColor: "#b9dced" }}>
          Register
        </Link>
      </div>
    </div>
  </div>
);

const styles = {
  container: {
    position: "relative",
    display: "flex",
    justifyContent: "center",
    alignItems: "center",
    minHeight: "100vh",
    backgroundImage: "url('https://media.istockphoto.com/id/1419724017/photo/car-rental-agency-employee-giving-car-keys-to-beautiful-young-woman.jpg?b=1&s=612x612&w=0&k=20&c=Qd2P0VYuQGmethhCvWzR51PgZmGEN9YaWl47PiYL-3Q=')",
    backgroundSize: "cover",
    backgroundPosition: "center",
    backgroundRepeat: "no-repeat",
    overflow: "hidden",
  },
  overlay: {
    position: "absolute",
    top: 0,
    left: 0,
    right: 0,
    bottom: 0,
    backgroundColor: "rgba(0, 0, 0, 0.6)", // Dark overlay for better readability
    display: "flex",
    flexDirection: "column",
    justifyContent: "center",
    alignItems: "center",
    textAlign: "center",
    color: "white",
    padding: "20px",
  },
  title: {
    fontSize: "4rem",
    fontWeight: "bold",
    marginBottom: "10px",
    color: "#ffffff",
  },
  highlight: {
    color: "#72c4ed",
  },
  subtitle: {
    fontSize: "1.5rem",
    fontWeight: "300",
    marginBottom: "30px",
    color: "#dfe6e9",
    maxWidth: "600px",
  },
  features: {
    display: "flex",
    justifyContent: "center",
    gap: "40px",
    marginBottom: "30px",
  },
  featureItem: {
    textAlign: "center",
    color: "#3e5161",
  },
  icon: {
    fontSize: "2.5rem",
    marginBottom: "10px",
  },
  featureText: {
    fontSize: "1rem",
    color: "#ffffff",
  },
  buttonContainer: {
    display: "flex",
    gap: "20px",
  },
  button: {
    display: "inline-block",
    padding: "15px 30px",
    backgroundColor: "#72c4ed",
    color: "black",
    fontSize: "1.2rem",
    fontWeight: "bold",
    borderRadius: "30px",
    textDecoration: "none",
    transition: "transform 0.3s ease, background-color 0.3s ease",
    cursor: "pointer",
  },
};

export default Home;
