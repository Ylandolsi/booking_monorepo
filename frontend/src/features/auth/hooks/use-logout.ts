import { useMutation, useQueryClient } from "@tanstack/react-query";
import { logout } from '@/features/auth'; 
import { toast } from "sonner";

export const useLogout = ({ 
  onSuccess 
}: { 
  onSuccess?: () => void;
} = {}) => {
  const queryClient = useQueryClient();
  
  return useMutation({
    mutationFn: logout,
    onSuccess: () => {
      queryClient.removeQueries({ queryKey: ['user'] });
      toast.success("Logged out successfully!");
      onSuccess?.();
    },
    onError: (error) => {
      console.error("Logout failed:", error);
      toast.error("Logout failed. Please try again.");
    },
  });
};