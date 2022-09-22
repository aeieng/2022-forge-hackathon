import { Col, Modal, Row, Select, Spin } from "antd";
import { useContext, useEffect, useState } from "react";
import { ModelContext } from "../context/ModelContext";
import { Building, Model } from "./ModelTable";

const AddModelModal = () => {
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
    if (
      modelToAdd !== undefined &&
      modelToAdd.discipline &&
      modelToAdd.building?.id
    ) {
      // POST add-model.
      fetch("https://localhost:5001/token")
        .then((response) => {
          if (response.ok) {
            return response.json();
          }
          throw response;
        })
        .then((token) => {
          fetch("https://localhost:5001/add-model", {
            method: "POST",
            headers: new Headers({
              Authorization: `Bearer ${token.accessToken}`,
              "Content-Type": "application/json",
            }),
            body: JSON.stringify({
              autodeskItemId: modelToAdd.autodeskItemId,
              autodeskProjectId: modelToAdd.autodeskProjectId,
              autodeskHubId: modelToAdd.autodeskHubId,
              buildingId: modelToAdd.building.id,
              name: modelToAdd.title,
              type: modelToAdd.discipline,
              revitVersion: "string",
            }),
          })
            .then((response) => {
              if (response.ok) {
                return response.json();
              }
              throw response;
            })
            .then((data) => console.log(data))
            .catch((error) => {
              console.error(error);
            });
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
      title={modelToAdd?.title}
    >
      <Row>
        <Col span={6}>Discipline</Col>
        <Col span={6}>
          <Select
            options={[
              { label: "Architectural", value: "architectural" },
              { label: "Electrical", value: "electrical" },
              { label: "Mechanical", value: "mechanical" },
              { label: "Structural", value: "structural" },
            ]}
            value={modelToAdd?.discipline}
            onSelect={(value: string) => {
              setModelToAdd((prev) =>
                prev !== undefined ? { ...prev, discipline: value } : prev
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
            value={modelToAdd?.building?.name}
            onSelect={(_: string, option: { label: string; value: string }) => {
              setModelToAdd((prev) =>
                prev !== undefined
                  ? {
                      ...prev,
                      building: { id: option.value, name: option.label },
                    }
                  : prev
              );
            }}
            style={{ width: "250px" }}
          />
        </Col>
      </Row>
    </Modal>
  );
};

export default AddModelModal;
