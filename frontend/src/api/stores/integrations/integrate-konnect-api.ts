import { api } from '@/api/utils';
import { useMutation } from '@tanstack/react-query';
import { authQueryKeys } from '@/api/auth';
import { AuthEndpoints } from '@/api/utils/auth-endpoints';

const integateWithKonnect = async (walledId: string): Promise<void> => {
  await api.post(AuthEndpoints.User.IntegrateKonnectWallet, {
    konnectWalletId: walledId,
  });
};

export const useIntegrateWithKonnect = () => {
  return useMutation({
    mutationFn: (walledId: string) => integateWithKonnect(walledId),
    meta: {
      invalidatesQuery: [authQueryKeys.currentUser()],
      successMessage: 'Successfully integrated with Konnect',
      errorMessage: 'Failed to integrate with Konnect',
    },
  });
};
