import { zodResolver } from '@hookform/resolvers/zod';
import { useForm } from 'react-hook-form';
import {
  resetPasswordSchema,
  useResetPassword,
  AUTH_PLACEHOLDERS,
  type ResetPasswordInput,
} from '@/features/auth';
import {
  CardContent,
  CardDescription,
  CardFooter,
  Link,
  Form,
  FormControl,
  FormField,
  FormItem,
  FormLabel,
  FormMessage,
  Button,
} from '@/components/ui';
import { PasswordInput } from '@/components/ui';

type ResetPasswordPageProps = {
  onSuccess: () => void;
  email: string;
  token: string;
};

export const ResetPasswordPage = ({
  onSuccess,
  email,
  token,
}: ResetPasswordPageProps) => {
  const resetPassword = useResetPassword({ onSuccess });

  const form = useForm<ResetPasswordInput>({
    resolver: zodResolver(resetPasswordSchema),
    defaultValues: {
      email: email,
      token: token,
      password: '',
      confirmPassword: '',
    },
  });

  function onSubmit(values: ResetPasswordInput) {
    resetPassword.mutate(values);
  }

  return (
    <>
      <CardDescription className="text-center sm:mx-auto sm:w-full sm:max-w-md">
        <h2 className="text-2xl font-semibold tracking-tight">
          Reset Password
        </h2>
        <p className="text-muted-foreground mx-2 mt-2">
          Enter your new password.
        </p>
      </CardDescription>
      <CardContent>
        <Form {...form}>
          <form
            onSubmit={form.handleSubmit(onSubmit)}
            className="space-y-4 lg:space-y-6"
          >
            <FormField
              control={form.control}
              name="password"
              render={({ field }) => (
                <FormItem>
                  <FormLabel>New Password</FormLabel>
                  <FormControl>
                    <PasswordInput
                      placeholder={AUTH_PLACEHOLDERS.password}
                      {...field}
                    />
                  </FormControl>
                  <FormMessage />
                </FormItem>
              )}
            />
            <FormField
              control={form.control}
              name="confirmPassword"
              render={({ field }) => (
                <FormItem>
                  <FormLabel>Confirm Password</FormLabel>
                  <FormControl>
                    <PasswordInput
                      placeholder={AUTH_PLACEHOLDERS.confirmPassword}
                      {...field}
                    />
                  </FormControl>
                  <FormMessage />
                </FormItem>
              )}
            />
            <Button
              type="submit"
              className="w-full"
              disabled={resetPassword.isPending}
            >
              {resetPassword.isPending ? 'Resetting...' : 'Reset Password'}
            </Button>
          </form>
        </Form>
      </CardContent>
      <CardFooter className="flex-col gap-2">
        <Link
          className="text-primary flex justify-center hover:underline"
          href="/auth/login"
        >
          Back to login
        </Link>
      </CardFooter>
    </>
  );
};
