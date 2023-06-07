import React from "react"; // , { useEffect, useState }
import { BrowserRouter as Router, Switch, Route } from "react-router-dom";

import { ProtectedRoute } from "./protected-route";
import Login from "./Login/containers/login-container";

import HomeList from "./Home/containers/home-list";

import { signOut } from "./Utils/auth";

function App() {
  const logingOut = async () => {
    /* Esta funcion es para autenticación con AWS Cognito utilizando API Gateway     
    await Auth.signOut();
    */
    await signOut();
    window.location.href = "/";
  };

  return (
    <Router>
      <Switch>
        <Route exact path="/login" component={Login} />
        <Route exact path="/" component={Login} />
        <Route exact path="/logout" component={logingOut} />
        <ProtectedRoute exact path="/home" component={HomeList} />
      </Switch>
    </Router>
  );
}

//export default withAuthenticator(App, true);
export default App;
