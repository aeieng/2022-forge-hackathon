import { Button, Col, Row } from "antd";
import HubTree from "../components/HubTree";

const Admin = () => {
  return (
    <>
      <Row gutter={16}>
        <Col span={8}>
          <Button
            onClick={() => {
              alert("todo");
            }}
          >
            Add to be processed
          </Button>
        </Col>
        <Col span={8}></Col>
        <Col span={8}>
          <Button
            onClick={() => {
              alert("todo");
            }}
          >
            RUN
          </Button>
        </Col>
      </Row>
      <Row gutter={16}>
        <Col span={8}>
          <HubTree />
        </Col>
        <Col span={8}>Models To Process</Col>
        <Col span={8}>Process Log</Col>
      </Row>
    </>
  );
};

export default Admin;
