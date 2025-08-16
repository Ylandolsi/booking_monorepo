import * as React from 'react';
import { cva, type VariantProps } from 'class-variance-authority';

import { cn } from '@/utils/cn';
import {
  AlertCircle,
  AlertTriangle,
  CheckCircle,
  Info,
  XCircle,
} from 'lucide-react';

const alertIconMap = {
  default: AlertCircle,
  destructive: XCircle,
  success: CheckCircle,
  warning: AlertTriangle,
  info: Info,
} as const;
// {React.createElement(alertIconMap['success'])}

const alertVariants = cva(
  'relative w-full rounded-lg border px-4 py-3 text-sm grid has-[>svg]:grid-cols-[calc(var(--spacing)*4)_1fr] grid-cols-[0_1fr] has-[>svg]:gap-x-3 gap-y-0.5 items-start [&>svg]:size-4 [&>svg]:translate-y-0.5 [&>svg]:text-current',
  {
    variants: {
      variant: {
        default: 'bg-card text-card-foreground',
        destructive:
          'text-destructive bg-card [&>svg]:text-current *:data-[slot=alert-description]:text-destructive/90',
        success:
          'bg-green-50 text-green-800 border-green-200 [&>svg]:text-green-600 *:data-[slot=alert-description]:text-green-700',
        warning:
          'bg-yellow-50 text-yellow-800 border-yellow-200 [&>svg]:text-yellow-600 *:data-[slot=alert-description]:text-yellow-700',
        info: 'bg-blue-50 text-blue-800 border-blue-200 [&>svg]:text-blue-600 *:data-[slot=alert-description]:text-blue-700',
      },
      size: {
        sm: 'px-3 py-2 text-xs',
        default: 'px-4 py-3 text-sm',
        lg: 'px-5 py-4 text-base',
      },
    },
    defaultVariants: {
      variant: 'default',
      size: 'default',
    },
  },
);

type AlertProps = React.ComponentProps<'div'> &
  VariantProps<typeof alertVariants>;

// <div className="py-10 px-4 ">
//   <Alert variant="destructive">
//     {React.createElement(alertIconMap['destructive'])}
//     <AlertDescription>
//       {mentorDetailsQuery.isError
//         ? 'Failed to load mentor details. Please try again later.'
//         : 'Failed to load availability. Please refresh the page.'}
//     </AlertDescription>
//   </Alert>
// </div>;
function Alert({ className, variant, ...props }: AlertProps) {
  return (
    <div
      data-slot="alert"
      role="alert"
      className={cn(alertVariants({ variant }), className)}
      {...props}
    />
  );
}

function AlertTitle({ className, ...props }: React.ComponentProps<'div'>) {
  return (
    <div
      data-slot="alert-title"
      className={cn(
        'col-start-2 line-clamp-1 min-h-4 font-medium tracking-tight',
        className,
      )}
      {...props}
    />
  );
}

function AlertDescription({
  className,
  ...props
}: React.ComponentProps<'div'>) {
  return (
    <div
      data-slot="alert-description"
      className={cn(
        'text-muted-foreground col-start-2 grid justify-items-start gap-1 text-sm [&_p]:leading-relaxed',
        className,
      )}
      {...props}
    />
  );
}

// Convenience wrapper components
interface ConvenienceAlertProps extends Omit<AlertProps, 'variant'> {}

function SuccessAlert(props: ConvenienceAlertProps) {
  return <Alert variant="success" {...props} />;
}

function ErrorAlert(props: ConvenienceAlertProps) {
  return <Alert variant="destructive" {...props} />;
}

function WarningAlert(props: ConvenienceAlertProps) {
  return <Alert variant="warning" {...props} />;
}

function InfoAlert(props: ConvenienceAlertProps) {
  return <Alert variant="info" {...props} />;
}

// Toast Alert with auto-dismiss
interface ToastAlertProps extends AlertProps {
  duration?: number;
  onDismiss?: () => void;
}

function ToastAlert({ duration = 5000, onDismiss, ...props }: ToastAlertProps) {
  const [isVisible, setIsVisible] = React.useState(true);

  React.useEffect(() => {
    if (duration > 0) {
      const timer = setTimeout(() => {
        setIsVisible(false);
        onDismiss?.();
      }, duration);

      return () => clearTimeout(timer);
    }
  }, [duration, onDismiss]);

  const handleClose = () => {
    setIsVisible(false);
    onDismiss?.();
  };

  if (!isVisible) return null;

  return <Alert {...props} />;
}

// Inline Alert for form validation
// interface InlineAlertProps extends Omit<AlertProps, 'size'> {
//   children: React.ReactNode;
// }

// function InlineAlert({
//   variant = 'destructive',
//   className,
//   children,
//   ...props
// }: InlineAlertProps) {
//   const IconComponent = variant ? alertIconMap[variant] : AlertCircle;

//   return (
//     <div
//       className={cn(
//         'flex items-center gap-2 text-xs',
//         variant === 'destructive' && 'text-red-600',
//         variant === 'success' && 'text-green-600',
//         variant === 'warning' && 'text-yellow-600',
//         variant === 'info' && 'text-blue-600',
//         className,
//       )}
//       {...props}
//     >
//       <IconComponent className="w-3 h-3 flex-shrink-0" />
//       <span>{children}</span>
//     </div>
//   );
// }

export { Alert, AlertTitle, AlertDescription, alertIconMap };
