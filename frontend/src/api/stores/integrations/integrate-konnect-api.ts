import { api } from '@/api/utils';
import { IntegrateKonnectWallet } from '@/api/utils/auth-endpoints';
import { useMutation } from '@tanstack/react-query';
import { authQueryKeys } from '@/api/auth';

const integateWithKonnect = async (walledId: string): Promise<void> => {
  await api.post(IntegrateKonnectWallet, {
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
