import { Col, Row } from "antd";
import HubTree from "../components/HubTree";

const Admin = () => {
  return (
    <Row gutter={16}>
      <Col span={8}>
        <HubTree />
      </Col>
      <Col span={8}>two</Col>
      <Col span={8}>three</Col>
    </Row>
  );
};

export default Admin;
