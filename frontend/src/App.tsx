import { Routes, Route } from "react-router-dom";
import Login from "./component/Login";
import Nav from "./component/Nav";
import Admin from "./component/Admin";
import Inputs from "./component/Inputs";
import Dashboard from "./component/Dashboard";
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
