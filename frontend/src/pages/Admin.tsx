import { createContext, Dispatch, SetStateAction, useState } from "react";
import { Button, Col, Row } from "antd";
import HubTree from "../components/HubTree";
import ModelTable, { Model } from "../components/ModelTable";

type IModelContext = {
  models: Model[];
  setModels: Dispatch<SetStateAction<Model[]>>;
};

export const ModelContext = createContext<IModelContext>({
  models: [],
  setModels: () => {
    return;
  },
});

const Admin = () => {
  const [models, setModels] = useState<Model[]>([]);

  return (
    <ModelContext.Provider value={{ models, setModels }}>
      <Row gutter={16}>
        <Col span={16} />
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
        <Col span={4}>
          <HubTree />
        </Col>
        <Col span={10}>
          <Row>
            <Col>
              <h2>Models</h2>
              <ModelTable />
            </Col>
          </Row>
          <Row>
            <h2>Activities</h2>
          </Row>
        </Col>
        <Col span={10}>
          <h2>Extraction Log</h2>
        </Col>
      </Row>
    </ModelContext.Provider>
  );
};

export default Admin;
