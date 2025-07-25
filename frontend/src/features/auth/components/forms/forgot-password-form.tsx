import { zodResolver } from '@hookform/resolvers/zod';
import { useForm } from 'react-hook-form';
import {
  forgotPasswordInputSchema,
  useForgotPassword,
  type ForgotPasswordInput,
  AUTH_PLACEHOLDERS,
} from '@/features/auth';

import {
  Button,
  Input,
  Form,
  FormControl,
  FormField,
  FormItem,
  FormLabel,
  FormMessage,
  Link,
  CardContent,
  CardDescription,
  CardFooter,
} from '@/components/ui';

type ForgotPasswordFormProps = {
  onSuccess: () => void;
};

export const ForgotPasswordForm = ({ onSuccess }: ForgotPasswordFormProps) => {
  const forgotPassword = useForgotPassword({ onSuccess });

  const form = useForm<ForgotPasswordInput>({
    resolver: zodResolver(forgotPasswordInputSchema),
    defaultValues: {
      email: '',
    },
  });

  function onSubmit(values: ForgotPasswordInput) {
    forgotPassword.mutate(values);
  }

  return (
    <>
      <CardDescription className="text-center sm:mx-auto sm:w-full sm:max-w-md">
        <h2 className="text-2xl font-semibold tracking-tight">
          Forgot your password?
        </h2>
        <p className="text-muted-foreground mx-2 mt-2">
          Enter your email address and we will send you a link to reset your
          password.
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
              name="email"
              render={({ field }) => (
                <FormItem>
                  <FormLabel>Email</FormLabel>
                  <FormControl>
                    <Input
                      type="email"
                      placeholder={AUTH_PLACEHOLDERS.email}
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
              disabled={forgotPassword.isPending}
            >
              {forgotPassword.isPending ? 'Sending...' : 'Send Reset Link'}
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
