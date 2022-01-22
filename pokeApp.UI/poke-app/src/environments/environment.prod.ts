import authInfo from '../../auth_config.json';

export const environment = {
  production: true,
  auth: {
    domain: authInfo.domain,
    clientId: authInfo.clientId,
    redirectUri: window.location.origin,
  },
};
