export default {
  env: {
    environment: "local", // local|dev|production
    local: {
      /* cognito: {
        REGION: "us-east-1",
        USER_POOL_ID: "5455",
        APP_CLIENT_ID: "54545",
        IDENTITY_POOL_ID: "5454",
      }, */
      apiGateway: {
        REGION: "us-east-1",
        URL: "https://api.launion.com.gt/",
      },
    },
    dev: {
      /* cognito: {
        REGION: "us-east-1",
        USER_POOL_ID: "65665",
        APP_CLIENT_ID: "65665",
        IDENTITY_POOL_ID: "65665",
      }, */
      apiGateway: {
        REGION: "us-east-1",
        URL: "https://api.aws",
      },
    },
    prod: {
      /* cognito: {
        REGION: "us-east-1",
        USER_POOL_ID: "654",
        APP_CLIENT_ID: "654",
        IDENTITY_POOL_ID: "654",
      }, */
      apiGateway: {
        REGION: "us-east-1",
        URL: "https://api.aws",
      },
    },
  },
};
