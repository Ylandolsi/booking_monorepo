import { env } from '@/config/env';
import { routeBuilder } from '@/config';
import { buildUrlWithParams } from '@/api/utils';
import { AuthEndpoints } from '../utils/auth-endpoints';

export const googleOIDC = async (returnUrl: string = routeBuilder.app.root()) => {
  const url = `${env.API_URL}/${AuthEndpoints.Google.Login}`;
  const returnUrlComplete = `${env.APP_URL}${returnUrl}`;
  const fullUrl = buildUrlWithParams(url, { returnUrl: returnUrlComplete });
  window.location.href = fullUrl;
};
