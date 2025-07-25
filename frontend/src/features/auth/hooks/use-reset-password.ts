import { useMutation } from "@tanstack/react-query";
import { resetPassword } from '@/features/auth'; 
import { toast } from "sonner";

export const useResetPassword = ({ onSuccess }: { onSuccess?: () => void } = {}) => {
    return useMutation({
        mutationFn: resetPassword,
        onSuccess: () => {
            console.log("password reset successfully");
            onSuccess?.();
            toast.success("Password reset successfully. Please login with your new password.");
        },
        onError: (error) => {
            console.error("Reset password failed:", error);
            toast.error("Failed to reset password. Please try again.");
        },
    });
};