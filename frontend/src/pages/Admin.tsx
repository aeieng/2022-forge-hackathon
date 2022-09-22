import { Col, Row } from "antd";
import ActivitiesTable from "../components/ActivitiesTable";
import ExtractionLog from "../components/ExtractionLog";
import HubTree from "../components/HubTree";
import ModelTable from "../components/ModelTable";
import { ModelContextProvider } from "../context/ModelContext";

const Admin = () => {
  return (
    <ModelContextProvider>
      <Row gutter={16}>
        <Col span={4}>
          <h2>Hub Tree</h2>
          <HubTree />
        </Col>
        <Col span={10}>
          <Row>
            <Col flex="auto">
              <h2>Models</h2>
              <ModelTable />
            </Col>
          </Row>
          <Row>
            <Col flex="auto">
              <h2>Activities</h2>
              <ActivitiesTable />
            </Col>
          </Row>
        </Col>
        <Col span={10}>
          <h2>Extraction Log</h2>
          <ExtractionLog />
        </Col>
      </Row>
    </ModelContextProvider>
  );
};

export default Admin;
