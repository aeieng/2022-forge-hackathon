import { Routes, Route } from "react-router-dom";
import { Button, Col, Layout, Row, Space } from "antd";

import Login from "./pages/Login";
import Admin from "./pages/Admin";
import Inputs from "./pages/Inputs";
import Dashboard from "./pages/Dashboard";
import Nav from "./components/Nav";
import "./App.css";
import User from "./components/User";

const { Header, Footer, Content } = Layout;

const App = () => {
  return (
    <Layout style={{ height: "100%" }}>
      <Header>
        <Row gutter={16}>
          <Col>
            <Nav />
          </Col>
          <Col flex="auto" />
          <Col>
            <Space>
              <Button>Add Building</Button>
              <User />
            </Space>
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
  );
};

export default App;
