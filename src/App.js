import React from "react";
import { BrowserRouter as Router, Routes, Route, Navigate } from "react-router-dom"; // Import Navigate for redirection
import Login from "./pages/Login";
import Register from "./pages/Register";
import ForgotPassword from "./pages/ForgotPassword";
import { AuthProvider } from "./components/AuthContext";
import { ToastContainer } from "react-toastify";
import "react-toastify/dist/ReactToastify.css";
import "@fontsource/roboto";
import RoadReadyHome from "./pages/RoadReadyHome";
import UserProfile from "./pages/UserProfile";
import Home from "./pages/Home";
import ReserveNow from "./pages/ReserveNow";
import Payment from "./pages/Payment";
import Success from "./pages/Success";
import HostRegister from "./pages/HostRegister";
import Reservations from "./pages/Reservations";
import SubmitProof from "./pages/SubmitProof";
import PostReview from "./pages/PostReview";


//import { styles } from "./Styles"; // Ensure correct path to styles.js
 // Assuming this is the initial landing page

const App = () => {
  return (
    <Router>
      <AuthProvider>
        <Routes>
          {/* Home page route (before login) */}
          <Route path="/" element={<Home />} />

          {/* Login page */}
          <Route path="/login" element={<Login />} />

          {/* Register page */}
          <Route path="/register" element={<Register />} />

          {/* Forgot Password page */}
          <Route path="/forgot-password" element={<ForgotPassword />} />

          {/* RoadReadyHome page after login */}
          <Route path="/roadreadyhome" element={<RoadReadyHome />} />

          {/* User Profile page */}
          <Route path="/profile" element={<UserProfile />} />
          <Route path="/reservations" element={<Reservations />} />
          
          <Route path="/reservenow/:id" element ={<ReserveNow />} />

          <Route path="/payment" element={<Payment />} />

          <Route path = "/host-register" element = {<HostRegister />} />
          <Route path="/submit-proof" element={<SubmitProof />} />
          <Route path="/postreview" element={<PostReview />} />

          {/* <Route path="/payment" element={<Payment />} />
          <Route path="/success" element={<Success />} /> */}

        </Routes>
        <ToastContainer />
      </AuthProvider>
    </Router>
  );
};

export default App;// Inline Styles
export const styles = {
    container: {
        fontFamily: "Roboto, sans-serif",
        backgroundColor: "#f4f4f9",
        margin: 0,
    },
    header: {
        display: "flex",
        justifyContent: "space-between",
        padding: "10px 20px",
        backgroundColor: "#003366",
        color: "white",
    },
    logo: {
        fontSize: "1.5rem",
        fontWeight: "bold",
    },
    nav: {
        display: "flex",
        gap: "15px",
    },
    navLink: {
        color: "white",
        textDecoration: "none",
    },
    browseButton: {
        color: "white",
        background: "none",
        border: "none",
        cursor: "pointer",
        textDecoration: "underline",
    },
    banner: {
        background: 'url("https://img.freepik.com/free-photo/young-couple-talking-sales-person-car-showroom_1303-15136.jpg?t=st=1733032231~exp=1733035831~hmac=f13067f740d6bfae309c44895742b8b3247d868277d961dab32ac78a7cde1be5&w=996") no-repeat center center/cover',
        color: "white",
        padding: "60px 20px",
        textAlign: "center",
    },
    searchBar: {
        display: "flex",
        gap: "10px",
        margin: "20px auto",
        justifyContent: "center",
    },
    input: {
        padding: "10px",
        fontSize: "1rem",
    },
    searchButton: {
        padding: "10px 20px",
        backgroundColor: "#007bff",
        color: "white",
        border: "none",
        cursor: "pointer",
    },
    featuredCars: {
        padding: "20px",
    },
    carGrid: {
        display: "grid",
        gridTemplateColumns: "repeat(auto-fill, minmax(300px, 1fr))",
        gap: "20px",
    },
    carCard: {
        background: "white",
        padding: "20px",
        border: "1px solid #ddd",
        textAlign: "center",
        borderRadius: "10px",
    },
    carImage: {
        maxWidth: "100%",
        borderRadius: "10px",
    },
    detailsButton: {
        margin: "5px",
        backgroundColor: "#007bff",
        color: "white",
        border: "none",
        padding: "10px",
        cursor: "pointer",
    },
    reserveButton: {
        margin: "5px",
        backgroundColor: "#28a745",
        color: "white",
        border: "none",
        padding: "10px",
        cursor: "pointer",
    },
    footer: {
        backgroundColor: "#003366",
        color: "white",
        padding: "20px",
        textAlign: "center",
    },
    footerLink: {
        color: "#ff9800",
        textDecoration: "none",
        margin: "0 5px",
    },
};

