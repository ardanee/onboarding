import React, { useState, useEffect } from "react";
import { Route, Redirect } from "react-router-dom";
import { isSignedIn } from "./Utils/auth";

/* import { Auth } from "aws-amplify"; */

export const ProtectedRoute = ({ component: Component, ...rest }) => {
  const [isLoading, setIsLoading] = useState(true);
  const [authenticated, setAuthenticated] = useState(false);

  useEffect(() => {
    checkUser();
  });

  async function checkUser() {
    try {
      /*
      Si la autenticación es por medio de Cognito, esta debería ser el código de valicadión.
      await Auth.currentAuthenticatedUser();
      await Auth.currentSession(); */
      const session = await isSignedIn();

      if (session.response) {
        setAuthenticated(true);
      } else {
        setAuthenticated(false);
      }
    } catch (e) {
      setAuthenticated(false);
    }
    setIsLoading(false);
  }
  return (
    <Route
      {...rest}
      render={(props) => {
        if (isLoading) {
          return <div>Cargando</div>;
        }
        if (authenticated) {
          return <Component {...props} />;
        } else {
          return (
            <Redirect
              to={{
                pathname: "/login",
                state: {
                  from: props.location,
                },
              }}
            />
          );
        }
      }}
    />
  );
};
