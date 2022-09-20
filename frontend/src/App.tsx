import { Routes, Route } from "react-router-dom";
import Login from "./pages/Login";
import Admin from "./pages/Admin";
import Inputs from "./pages/Inputs";
import Dashboard from "./pages/Dashboard";
import Nav from "./components/Nav";
import "./App.css";

const App = () => {
  return (
    <>
      <Nav />
      <Routes>
        <Route path="/">
          <Route index element={<Login />} />
          <Route path="/admin" element={<Admin />} />
          <Route path="/inputs" element={<Inputs />} />
          <Route path="/dashboard" element={<Dashboard />} />
        </Route>
      </Routes>
    </>
  );
};

export default App;
