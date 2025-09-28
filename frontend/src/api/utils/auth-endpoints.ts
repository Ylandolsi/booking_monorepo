const GoogleLogin = 'users/login/google';
const GoogleLoginCallback = 'users/login/google/callback';
// Authentication and Authorization
const Login = 'users/login';
const Register = 'users/register';
const RefreshAccessToken = 'users/refresh-token';
const VerifyEmail = 'users/verify-email';
const ResendVerificationEmail = 'users/resend-verification-email';
const Logout = 'users/logout';
const ChangePassword = 'users/change-password';

const ForgotPassword = 'users/forgot-password';
const ResetPasword = 'users/reset-password';

const IntegrateKonnectWallet = 'users/integrate/konnect';

const GetUser = 'users/{userSlug}';
const GetCurrentUser = 'users/me';

export {
  GoogleLogin,
  GoogleLoginCallback,
  Login,
  Register,
  RefreshAccessToken,
  VerifyEmail,
  ResendVerificationEmail,
  Logout,
  ChangePassword,
  ForgotPassword as ResetPasswordSendToken,
  ResetPasword as ResetPasswordVerify,
  GetCurrentUser,
  GetUser,
  IntegrateKonnectWallet,
};
