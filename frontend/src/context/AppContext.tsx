import {
  createContext,
  Dispatch,
  SetStateAction,
  useState,
  PropsWithChildren,
  useEffect,
} from "react";

export type Token = {
  accessToken: string;
  expiresAt: string;
};

type Context = {
  token: Token;
  setToken: Dispatch<SetStateAction<Token>>;
};

export const AppContext = createContext<Context>({
  token: {
    accessToken: "",
    expiresAt: "",
  },
  setToken: () => {},
});

export const AppContextProvider = ({ children }: PropsWithChildren) => {
  const [token, setToken] = useState<Token>({
    accessToken: "",
    expiresAt: "",
  });

  useEffect(() => {
    // if (!token.accessToken)
    fetch("https://localhost:5001/token")
      .then((response) => {
        if (response.ok) {
          return response.json();
        }
        throw response;
      })
      .then((data) => {
        setToken(data);
      });
  }, []);

  return (
    <AppContext.Provider value={{ token, setToken }}>
      {children}
    </AppContext.Provider>
  );
};
