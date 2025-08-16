import { buildUrlWithParams } from '../../../lib/api-client';
import * as Endpoints from '@/lib/user-endpoints.ts';
import { env } from '@/config/env';
import { routeBuilder } from '@/config';

export const googleOIDC = async (
  returnUrl: string = routeBuilder.app.root(),
) => {
  const url = `${env.API_URL}/${Endpoints.GoogleLogin}`;
  const returnUrlComplete = `${env.APP_URL}${returnUrl}`;
  const fullUrl = buildUrlWithParams(url, { returnUrl: returnUrlComplete });
  window.location.href = fullUrl;
};
