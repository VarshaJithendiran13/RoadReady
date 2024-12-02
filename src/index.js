import React from "react";
import ReactDOM from "react-dom/client";
import "./index.css";
import App from "./App";
import reportWebVitals from "./reportWebVitals";
import { BrowserRouter } from "react-router-dom";

// Create the root for React 18+
const root = ReactDOM.createRoot(document.getElementById("root"));

root.render(
  
    <App />
  
);

// Optional: For measuring app performance
reportWebVitals();
