import axios from "axios";

let accessToken = "";
let currentUsername = "";

/**
 * @param {string} token
 */
export function setAccessToken(token) {
  accessToken = token;
  document.cookie = `access_token=${token}; path=/;`;
}

export function getAccessToken() {
  if (typeof window !== "undefined") {
    const match = document.cookie.match(/(?:^|; )access_token=([^;]*)/);
    return match ? match[1] : null;
  }
}

/**
 * @param {string} username
 */
export function setUsername(username) {
  currentUsername = username;

  document.cookie = `username=${username}; path=/;`;
}

export function getUsername() {
  if (typeof document === "undefined") return null;
  const match = document.cookie.match(/(?:^|; )username=([^;]*)/);
  return match ? match[1] : null;
}

const apiClient = axios.create({
  baseURL: "https://localhost:7154",
  withCredentials: true,
  headers: {
    "Content-Type": "application/json",
    Authorization: `Bearer ${getAccessToken() || ""}`,
  },
});

apiClient.interceptors.request.use((config) => {
  if (accessToken) {
    config.headers["Authorization"] = `Bearer ${accessToken}`;
  }
  return config;
});

apiClient.interceptors.response.use(
  (response) => response,
  (error) => {
    if (error.response && error.response.status === 401) {
      window.location.href = "/promo";
    }
    return Promise.reject(error);
  }
);

export default apiClient;
