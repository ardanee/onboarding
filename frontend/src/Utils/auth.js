export const isSignedIn = async () => {
  let signedIn = { response: false, accessToken: "" };

  try {
    // Retreive the credentials
    const credentials = await sessionStorage.getItem("userData");

    if (credentials) {
      let objCredentials = JSON.parse(credentials);

      signedIn["response"] = true;
      signedIn["accessToken"] = objCredentials.access_token;
      signedIn["codigoUsuario"] = objCredentials.codigoUsuario;
      signedIn["userName"] = objCredentials.userName;
      signedIn["roles"] = objCredentials.roles;
    }
  } catch (error) {
    signedIn["message"] = "Error al obtener los datos de sesión!" + error;
  }
  return signedIn;
};

export const signIn = async (authInfo) => {
  if (!authInfo.error) {
    try {
      // Store the credentials
      sessionStorage.setItem("userData", JSON.stringify(authInfo));
    } catch (error) {
      // Error saving data
      //Alert.alert("Error de ejecución", errorList["006"]);
      return null;
    }
    return true;
  } else {
    await signOut(); //Eliminamos todas las sesiones para evitar sesiones activas zombies.
    return false;
  }
};

export const signOut = async () => {
  try {
    sessionStorage.setItem("userData", "");
    sessionStorage.clear();
  } catch (error) {
    alert("ERROR CERRAR SESION" + error);
  }

  //window.location.href = "/login";

  return true;
};
