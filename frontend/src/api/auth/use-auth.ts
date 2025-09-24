import { useUser, useLogin, useRegister, useLogout, useForgotPassword, useResetPassword } from '@/api/auth';
import { useAppNavigation } from '@/hooks';

export const useAuth = () => {
  const navigate = useAppNavigation();
  const { data: currentUser, isLoading, error } = useUser();
  const login = useLogin();
  const register = useRegister();

  const logout = useLogout({
    onSuccess: () => {
      navigate.goToLogin();
    },
  });
  const forgotPassword = useForgotPassword();
  const resetPassword = useResetPassword();

  return {
    // State
    currentUser,
    isLoading,
    error,
    isAuthenticated: !!currentUser,

    // Actions
    login: login.mutate,
    register: register.mutate,
    logout: logout.mutate,
    forgotPassword: forgotPassword.mutate,
    resetPassword: resetPassword.mutate,

    // Loading states
    isLoggingIn: login.isPending,
    isRegistering: register.isPending,
    isLoggingOut: logout.isPending,
    isForgettingPassword: forgotPassword.isPending,
    isResettingPassword: resetPassword.isPending,
  };
};
