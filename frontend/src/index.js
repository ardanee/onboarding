import React from "react";
import ReactDOM from "react-dom";
//import "./index.css";
import App from "./App";
import * as serviceWorker from "./serviceWorker";

/* Este código comentado sirve para autenticación con AWS Cognito utilizando API Gateway     
    
import Amplify from "aws-amplify";
import config from "./config";


Amplify.configure({
  Auth: {
    mandatorySignIn: true,
    region: config.env[config.env.environment].cognito.REGION,
    userPoolId: config.env[config.env.environment].cognito.USER_POOL_ID,
    identityPoolId: config.env[config.env.environment].cognito.IDENTITY_POOL_ID,
    userPoolWebClientId:
      config.env[config.env.environment].cognito.APP_CLIENT_ID,
  },
  API: {
    endpoints: [
      {
        name: config.env.environment,
        endpoint: config.env[config.env.environment].apiGateway.URL,
        region: config.env[config.env.environment].apiGateway.REGION,
      },
    ],
  },
});

*/

ReactDOM.render(<App />, document.getElementById("root"));

serviceWorker.unregister();
