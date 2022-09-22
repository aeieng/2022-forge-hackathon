import {
  createContext,
  Dispatch,
  SetStateAction,
  useState,
  PropsWithChildren,
  useEffect,
  useContext,
} from "react";
import { AppContext } from "./AppContext";

export type Building = {
  id: string;
  name: string;
};

export type ModelQuery = {
  autodeskHubId: string;
  autodeskItemId: string;
  autodeskProjectId: string;
  buildingId: string;
  name: string;
  revitVersion: string;
  type: string;
};

export type ModelResponse = {
  id: string;
} & ModelQuery;

export type Model = {
  buildingName?: string;
} & ModelResponse;

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
  const { token } = useContext(AppContext);
  const [models, setModels] = useState<Model[]>([]);
  const [modelToAdd, setModelToAdd] = useState<Model>();

  useEffect(() => {
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
        setModels(
          data.map(
            (model: ModelResponse): Model => ({
              id: model.id,
              name: model.name,
              buildingId: model.buildingId,
              // Building name not provided, using id for now.
              buildingName: model.buildingId,
              type: model.type,
              autodeskItemId: model.autodeskItemId,
              autodeskProjectId: model.autodeskProjectId,
              autodeskHubId: model.autodeskHubId,
              revitVersion: "",
            })
          )
        );
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
