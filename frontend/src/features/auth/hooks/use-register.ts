import { useMutation } from "@tanstack/react-query";
import { registerUser } from '@/features/auth'; 
import { toast } from "sonner";

export const useRegister = ({ 
  onSuccess 
}: { 
  onSuccess?: () => void;
} = {}) => {  
  return useMutation({
    mutationFn: registerUser,
    onSuccess: () => {
      toast.success("Registration successful! Please check your email for verification.");
      onSuccess?.();
    },
    onError: (error) => {
      console.error("Registration failed:", error);
      toast.error("Registration failed. Please try again.");
    },
  });
};