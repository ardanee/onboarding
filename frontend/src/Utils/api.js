import config from "../config";

import { isSignedIn, signOut } from "./auth";
import axios from "axios";

console.log("tha faking config xd", config);

let BASE_API = config.env[config.env.environment].apiGateway.URL;

class Api {
  state = {
    token: {},
  };

  async baseApi() {
    return BASE_API;
  }

  async Login(user, pass, type) {
    const fetchEndpoint = `${BASE_API}token`;

    var details = {
      userName: user,
      password: pass,
      auth_type: type,
      grant_type: "password",
    };

    var formBody = [];
    for (var property in details) {
      var encodedKey = encodeURIComponent(property);
      var encodedValue = encodeURIComponent(details[property]);
      formBody.push(encodedKey + "=" + encodedValue);
    }
    formBody = formBody.join("&");

    const query = await fetch(fetchEndpoint, {
      method: "POST",
      headers: {
        "Content-Type": "application/x-www-form-urlencoded;charset=UTF-8",
      },
      body: formBody,
    });

    const request = await query.json();
    console.log("API[Login]: DATOS DE SESSION (TOKEN REQUEST)", request);
    return request;
  }

  //**METODOS GENERALES***********//

  async fetchGetRequest(uri) {
    const fetchEndpoint = `${BASE_API}${uri}`;

    const request = await this.realizarPeticionGet(fetchEndpoint);
    console.log(`API[${uri}]: `, request);

    return request;
  }

  async realizarPeticionGet(endPoint) {
    let auth = await isSignedIn();
    const fetchEndpoint = endPoint;

    const query = await fetch(fetchEndpoint, {
      headers: {
        Authorization: "bearer " + auth.accessToken,
      },
    });

    const request = await this.processResponse(query);
    if (request.statusCode === 401) {
      window.location.href = "/login";
    }

    return request;
  }

  async post(uri, formData) {
    //Usamos Axios
    let auth = await isSignedIn();

    const fetchEndpoint = `${BASE_API}${uri}`;

    const request = await axios.post(fetchEndpoint, formData, {
      headers: { Authorization: "bearer " + auth.accessToken },
    });
    console.log(`REQUEST POST AXIOS [${uri}]`, request);

    return request;
  }

  async realizarPeticionPostPut(fetchEndpoint, sendData, requestType) {
    fetchEndpoint = `${BASE_API}${fetchEndpoint}`;

    let auth = await isSignedIn();

    var formBody = [];
    for (var property in sendData) {
      var encodedKey = encodeURIComponent(property);
      var encodedValue = encodeURIComponent(sendData[property]);
      formBody.push(encodedKey + "=" + encodedValue);
    }
    formBody = formBody.join("&");

    const query = await fetch(fetchEndpoint, {
      method: requestType,
      headers: {
        "Content-Type": "application/x-www-form-urlencoded;charset=UTF-8",
        Authorization: "Bearer " + auth.accessToken,
      },
      body: formBody,
    });

    const request = await this.processResponse(query);
    return request;
  }

  ////////////////////////Generales

  async processResponse(response) {
    const statusCode = response.status;
    let data;
    try {
      data = response.json();
    } catch (e) {
      data = {};
    }

    let promiseResponse = await Promise.all([statusCode, data]).then((res) => ({
      statusCode: res[0],
      data: res[1],
    }));

    if (promiseResponse.statusCode === 401) {
      await signOut();
    }

    return promiseResponse;
  }

  async fetchGetRequestWithParams(uri, params) {
    const urlParameters =
      params != null
        ? "?" +
          Object.keys(params)
            .map(
              (k) => encodeURIComponent(k) + "=" + encodeURIComponent(params[k])
            )
            .join("&")
        : "";

    const fetchEndpoint = `${BASE_API}${uri}${urlParameters}`;

    const request = await this.realizarPeticionGet(fetchEndpoint);
    console.log(`API[${fetchEndpoint}]: `, request);

    if (request.statusCode === 401) {
      if (await signOut()) {
        window.location.href = "/login";
      }

      return false;
    }

    return request;
  }
}

export default new Api();
