import { Routes, Route } from "react-router-dom";
import { Col, Layout, Row } from "antd";

import Login from "./pages/Login";
import Admin from "./pages/Admin";
import Inputs from "./pages/Inputs";
import Dashboard from "./pages/Dashboard";
import Nav from "./components/Nav";
import User from "./components/User";
import { AppContextProvider } from "./context/AppContext";
import "./App.css";

const { Header, Footer, Content } = Layout;

const App = () => {
  return (
    <AppContextProvider>
      <Layout style={{ height: "100%" }}>
        <Header style={{ backgroundColor: "white" }}>
          <Row gutter={16}>
            <Col>
              <Nav />
            </Col>
            <Col flex="auto" />
            <Col>
              <User />
            </Col>
          </Row>
        </Header>
        <Content style={{ height: "100%", padding: "2rem" }}>
          <Routes>
            <Route path="/">
              <Route index element={<Login />} />
              <Route path="admin" element={<Admin />} />
              <Route path="inputs" element={<Inputs />} />
              <Route path="dashboard" element={<Dashboard />} />
            </Route>
          </Routes>
        </Content>
        <Footer style={{ textAlign: "center" }}>AEI 2022</Footer>
      </Layout>
    </AppContextProvider>
  );
};

export default App;
