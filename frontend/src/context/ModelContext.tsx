import {
  createContext,
  Dispatch,
  SetStateAction,
  useState,
  PropsWithChildren,
  useEffect,
} from "react";
import { Model } from "../components/ModelTable";
import { ModelResponse } from "../query/Model";

type Context = {
  models: Model[];
  setModels: Dispatch<SetStateAction<Model[]>>;
  modelToAdd: Model | undefined;
  setModelToAdd: Dispatch<SetStateAction<Model | undefined>>;
};

export const ModelContext = createContext<Context>({
  models: [],
  setModels: () => {},
  modelToAdd: undefined,
  setModelToAdd: () => {},
});

export const ModelContextProvider = ({ children }: PropsWithChildren) => {
  const [models, setModels] = useState<Model[]>([]);
  const [modelToAdd, setModelToAdd] = useState<Model>();

  useEffect(() => {
    fetch("https://localhost:5001/token")
      .then((response) => {
        if (response.ok) {
          return response.json();
        }
        throw response;
      })
      .then((token) => {
        fetch("https://localhost:5001/models", {
          method: "GET",
          headers: new Headers({
            Authorization: `Bearer ${token.accessToken}`,
            "Content-Type": "application/json",
          }),
        })
          .then((response) => {
            if (response.ok) {
              return response.json();
            }
            throw response;
          })
          .then((data: ModelResponse[]) => {
            console.log(data);
            setModels((prev) =>
              data.map((model: ModelResponse) => {
                const m: Model = {
                  key: model.id,
                  title: model.name,
                  building: {
                    id: model.buildingId,
                    name: model.buildingId,
                  },
                  discipline: model.type,
                  autodeskItemId: model.autodeskItemId,
                  autodeskProjectId: model.autodeskProjectId,
                  autodeskHubId: model.autodeskHubId,
                };
                return m;
              })
            );
          })
          .catch((error) => {
            console.error(error);
          });
      })
      .catch((error) => {
        console.error(error);
      });
  }, []);

  return (
    <ModelContext.Provider
      value={{ models, setModels, modelToAdd, setModelToAdd }}
    >
      {children}
    </ModelContext.Provider>
  );
};
