import { Col, Modal, Row, Select, Spin } from "antd";
import { useContext, useEffect, useState } from "react";
import { AppContext } from "../context/AppContext";
import {
  Building,
  Model,
  ModelContext,
  ModelQuery,
} from "../context/ModelContext";

const AddModelModal = () => {
  const { token } = useContext(AppContext);
  const { setModels, modelToAdd, setModelToAdd } = useContext(ModelContext);
  const [loading, setLoading] = useState(true);
  const [buildings, setBuildings] = useState([]);

  useEffect(() => {
    fetch("https://localhost:5001/buildings")
      .then((response) => response.json())
      .then((data) => setBuildings(data))
      .finally(() => setLoading(false));
  }, []);

  const addModel = () => {
    if (modelToAdd !== undefined && modelToAdd.type && modelToAdd.buildingId) {
      const payload: ModelQuery = {
        autodeskItemId: modelToAdd.autodeskItemId,
        autodeskProjectId: modelToAdd.autodeskProjectId,
        autodeskHubId: modelToAdd.autodeskHubId,
        buildingId: modelToAdd.buildingId,
        name: modelToAdd.name,
        type: modelToAdd.type,
        revitVersion: "",
      };
      // POST add-model.
      fetch("https://localhost:5001/add-model", {
        method: "POST",
        headers: new Headers({
          Authorization: `Bearer ${token.accessToken}`,
          "Content-Type": "application/json",
        }),
        body: JSON.stringify(payload),
      })
        .then((response) => {
          if (response.ok) {
            return;
          }
          throw response;
        })
        .catch((error) => {
          console.error(error);
        });

      // Update the context.
      setModels((prev: Model[]) => [...prev, modelToAdd]);
      // Close the modal.
      setModelToAdd(undefined);
    }
  };

  if (loading) {
    return <Spin />;
  }

  return (
    <Modal
      open={modelToAdd !== undefined}
      onCancel={() => setModelToAdd(undefined)}
      onOk={addModel}
      title={modelToAdd?.name}
    >
      <Row>
        <Col span={6}>Discipline</Col>
        <Col span={6}>
          <Select
            options={[
              { label: "Architectural", value: "Architectural" },
              { label: "Electrical", value: "Electrical" },
              { label: "Mechanical", value: "Mechanical" },
              { label: "Structural", value: "Structural" },
            ]}
            value={modelToAdd?.type}
            onSelect={(value: string) => {
              setModelToAdd((prev) =>
                prev !== undefined ? { ...prev, type: value } : prev
              );
            }}
            style={{ width: "250px" }}
          />
        </Col>
      </Row>
      <Row>
        <Col span={6}>Building</Col>
        <Col span={6}>
          <Select
            options={buildings.map((b: Building) => ({
              label: b.name,
              value: b.id,
            }))}
            value={modelToAdd?.buildingId}
            onSelect={(_: string, option: { label: string; value: string }) => {
              setModelToAdd((prev) => {
                const next: Model = {
                  ...(prev as Model),
                  buildingId: option.value,
                  buildingName: option.label,
                };
                return next;
              });
            }}
            style={{ width: "250px" }}
          />
        </Col>
      </Row>
    </Modal>
  );
};

export default AddModelModal;
