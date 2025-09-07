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

// Experience
const GetUserExperiences = 'users/experiences{userSlug}';
const AddExperience = 'users/experiences';
const UpdateExperience = 'users/experiences/{experienceId}';
const DeleteExperience = 'users/experiences/{experienceId}';

// Education
const GetUserEducations = 'users/educations/{userSlug}';
const AddEducation = 'users/educations';
const UpdateEducation = 'users/education/{educationId}';
const DeleteEducation = 'users/education/{educationId}';

// Expertise
const GetUserExpertises = 'users/expertises/{userSlug}';
const GetAllExpertises = 'expertises';
const UpdateUserExpertise = 'users/expertises';

// Language
const GetUserLanguages = 'users/languages/{userSlug}';
const GetAllLanguages = 'languages';
const UpdateUserLanguages = 'users/languages';

// Profile
const UpdateBasicInfo = 'users/profile/basic-info';
const UpdateProfilePicture = 'users/profile/picture';
const UpdateSocialLinks = 'users/profile/social-links';

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
  GetUserExperiences,
  IntegrateKonnectWallet,
  AddExperience,
  UpdateExperience,
  DeleteExperience,
  GetUserEducations,
  AddEducation,
  UpdateEducation,
  DeleteEducation,
  GetUserExpertises,
  GetAllExpertises,
  UpdateUserExpertise,
  GetUserLanguages,
  GetAllLanguages,
  UpdateUserLanguages,
  UpdateBasicInfo,
  UpdateProfilePicture,
  UpdateSocialLinks,
};
