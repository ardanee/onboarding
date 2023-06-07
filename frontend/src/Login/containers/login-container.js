import React, { useState } from "react";
import { withRouter } from "react-router-dom";
import LoginForm from "../components/login-form";
import { toast } from "react-toastify";

import { signIn } from "../../Utils/auth";

import API from "../../Utils/api";

function LoginContainer(props) {
  /* const [cognitoUserRequest, setCognitoUserRequest] = useState(null);
  const [requireNewPass, setRequireNewPAss] = useState(false); */

  const [user, setUser] = useState("");
  const [password, setPassword] = useState("");
  const [isLoading, setIsLoading] = useState(false);

  /* const [confirmNewPassword, setConfirmNewPassword] = useState(""); */

  /* Esta funcion es para autenticación con AWS Cognito utilizando API Gateway     
  Esta función hace el login con Cognito AWS cuando es requerido su cambio de pass.

   const onClickLogin = async () => {
    //Con Amplify
    setIsLoading(true);
    if (!requireNewPass) {
      setRequireNewPAss(false);
    }

    try {
      const request = await Auth.signIn(user, password);
      setCognitoUserRequest(request);
      //this.props.userHasAuthenticated(true);
      setIsLoading(false);

      //TODO
      if (request.challengeName === "NEW_PASSWORD_REQUIRED") {
        setRequireNewPAss(true);
        return;
      }
      props.history.push(`/dashboard`);
    } catch (e) {
      console.log(e);
      if (e.code === "PasswordResetRequiredException") {
        setRequireNewPAss(true);
      } else {
        alert(e.message);
      }
      setIsLoading(false);
    }
  }; */

  /* Esta funcion es para autenticación con AWS Cognito utilizando API Gateway     
  Esta función confirma la contraseña de un usuario de Cognito cuando es requerido su cambio de pass.
    
  const onClickConfirmNewPass = () => {
    setIsLoading(true);
    if (
      password === confirmNewPassword &&
      confirmNewPassword.length > 0 &&
      cognitoUserRequest != null
    ) {
      Auth.completeNewPassword(cognitoUserRequest, password)
        .then((user) => {
          onClickLogin();
          //NotificationManager.success("Please login", "Password Changed!");
          //showNewPasswordModal(false);
          //hideAuthLoader();
        })
        .catch((err) => {
          console.log("ERROR:", err);
          alert(err.message);
          // NotificationManager.error(err);
          //hideAuthLoader();
        });
    } else {
      alert("Verifique que su contreseña sea correcta");
    }
    setIsLoading(false);
    return;
  };
  */

  const onClickLogin = async () => {
    setIsLoading(true);

    try {
      await API.Login(user, password, 1).then(async (result) => {
        if (await signIn(result)) {
          props.history.push(`/home`);
        } else {
          setIsLoading(false);
          toast.error(
            "Su usuario o contraseña no son correctos, intentalo de nuevo."
          );
        }
      });
    } catch (e) {
      alert(
        "En este momento no se puede realizar tu solicitud, intentalo más tarde o llama a tu administrador"
      );
    } finally {
      setIsLoading(false);
    }
  };

  return (
    <LoginForm
      onClickLogin={onClickLogin}
      onChangeUser={(value) => setUser(value.trim())}
      onChangePassword={(value) => setPassword(value)}
      isLoading={isLoading}
      /* requireNewPass={requireNewPass} */
      /* onClickConfirmNewPass={onClickConfirmNewPass} */
      /* onChangeConfirmNewPassword={(value) => setConfirmNewPassword(value)} */
      /* newPasswordConfirmed={
        password === confirmNewPassword && password.length >= 6
      } */
    />
  );
}

export default withRouter(LoginContainer);
