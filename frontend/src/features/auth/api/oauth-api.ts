import { buildUrlWithParams } from '../../../lib/api-client';
import * as Endpoints from '@/lib/endpoints.ts';
import { env } from '@/config/env';
import { paths } from '@/config/paths';

export const googleOIDC = async (returnUrl: string = paths.home.getHref()) => {
  const url = `${env.API_URL}/${Endpoints.GoogleLogin}`;
  const returnUrlComplete = `${env.APP_URL}${returnUrl}`;
  const fullUrl = buildUrlWithParams(url, { returnUrl: returnUrlComplete });
  window.location.href = fullUrl;
};
