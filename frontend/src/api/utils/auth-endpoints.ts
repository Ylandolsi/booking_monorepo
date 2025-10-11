const BASE = 'users';

export const AuthEndpoints = {
  Google: {
    Login: `${BASE}/login/google`,
    Callback: `${BASE}/login/google/callback`,
  },
  Local: {
    Login: `${BASE}/login`,
    Register: `${BASE}/register`,
    ForgotPassword: `${BASE}/forgot-password`,
    ResetPassword: `${BASE}/reset-password`,
  },
  User: {
    Current: `${BASE}/me`,
    BySlug: (userSlug: string) => `${BASE}/${userSlug}`,
    IntegrateKonnectWallet: `${BASE}/integrate/konnect`,
  },
  Tokens: {
    RefreshAccessToken: `${BASE}/refresh-token`,
  },
  Email: {
    Verify: `${BASE}/verify-email`,
    ResendVerification: `${BASE}/resend-verification-email`,
  },
  Password: {
    Change: `${BASE}/change-password`,
  },
  Logout: `${BASE}/logout`,
} as const;
