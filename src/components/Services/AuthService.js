import axios from "axios";

const API_URL = "https://localhost:7173/api/Auth";

const login = async (email, password,role) => {
  try {
    console.log("service called");
    console.log(email, password,role);
    const response = await axios.post(
      `https://localhost:7173/api/Auth/login`,
      {
        email,
        password,
        role,
      }
    );

    console.log(`response ${response}`);
    const token = response.data.token;

    // Save the token in localStorage
    localStorage.setItem("token", token);

    return token;
  } catch (error) {
    console.error("Error logging in:", error);
    throw error;
  }
};

const getProtectedData = async () => {
  try {
    const token = localStorage.getItem("token");

    const response = await axios.get(
      "https://localhost:7173/api/protected-endpoint",
      {
        headers: {
          Authorization: `Bearer ${token}`,
        },
      }
    );

    return response.data;
  } catch (error) {
    console.error("Error fetching protected data:", error);
    throw error;
  }
};

export { login, getProtectedData };
