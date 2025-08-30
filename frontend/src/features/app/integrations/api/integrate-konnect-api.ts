import { api } from '@/lib';
import { IntegrateKonnectWallet } from '@/lib/user-endpoints';
import { useMutation } from '@tanstack/react-query';

const integateWithKonnect = async (walledId: string): Promise<void> => {
  await api.post(IntegrateKonnectWallet, {
    konnectWalletId: walledId,
  });
};

export const useIntegrateWithKonnect = () => {
  return useMutation({
    mutationFn: (walledId: string) => integateWithKonnect(walledId),
    meta: {
      //   invalidatesQuery: [mentorQueryKeys.mentorProfile(user?.slug)],
      successMessage: 'Successfully integrated with Konnect',
      // errorMessage: 'Failed to become mentor',
    },
  });
};
