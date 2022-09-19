import { Routes, Route } from "react-router-dom";
import Login from "./components/login";
import Nav from "./components/nav";
import "./App.css";

const App = () => {
  return (
    <div>
      <Routes>
        <Route path="/">
          <Route index element={<Login />} />
          <Route path=":tabId" element={<Nav />} />
        </Route>
      </Routes>
    </div>
  );
};

export default App;
