// import { cn } from '@/lib';
// import { IPhoneMockup } from 'react-device-mockup';

interface MobileContainerProps {
  children: React.ReactNode;
  className?: string;
  screenWidth?: number;
}

export function MobileContainer({ screenWidth, children }: MobileContainerProps) {
  return (
    // <IPhoneMockup screenWidth={screenWidth || 400}>
    //   <div className={cn('bg-background flex h-full w-full flex-col items-center space-y-4 overflow-y-auto p-4 text-center', children)}>
    //     {children}
    //   </div>
    // </IPhoneMockup>

    <div className="border-border h-[812px] w-[375px] overflow-hidden rounded-xl border-2 shadow-2xl">
      <div className="h-full w-full overflow-x-hidden overflow-y-auto">
        {/* <!-- Cover image --> */}
        {/* <div
          class="h-48 bg-cover bg-center"
          style="
                  background-image: url('https://lh3.googleusercontent.com/aida-public/AB6AXuBNhWa7uSVI4TTUmVpCsNL2qj_0P5GDYoeHb2Zu0rmeMlVnz_81k1Ar3ZkoB2tupLQB0wyv5gmbeTm6carAzer8W5V0SI1S0LtQxp63kGxfz7MmQeamsLjpFq5Sk7YzDkdVMFtTfQn3Oc9AEy-QjRLNgcDJDuf_s8DLrxZuHTWbM4o3Hoh0gnaYJ9nzhutTLSSn4hXMA1AIvqpQy9JTSva-qgIvlrf3vGNabiVa8O_tk-YS2Rizao-knqNAMYU2kUVuEIpd1xGIqAE1');
                " 
        ></div>
                */}

        {/* <!-- Profile picture -mt16 bech tji above the cover phoyo  --> */}
        {/* 
        <div className="-mt-16 flex flex-col items-center p-6">
          <img
            alt="Sophia Carter"
            className="border-background-light dark:border-background-dark size-28 rounded-full border-4 object-cover"
            src="https://lh3.googleusercontent.com/aida-public/AB6AXuAVVUW7VSR5qKZvyiFJrocy8LklzRipqoFqpbkZCKyOmfv7JmjP8JHP9m_1XYbqwN-KKIeIPmcZBUOmFRgjxbJHdC5HxztN7NS8ZUr4P-UF91F9iYl6yoixDD-R-ViQ3gAu8FL9ZOJOcYFX6hKNuNedrFWV5RTbYVLnMQLXa4NX2EE1Y5bG63rMkkjBvNqg1N4IPDHAru9vJ6BRLLMb39w2wqmL0RyA16I57e3Dby5l7DSSisuJvzlzRNcXi7R2ecY5NBo4CXAaoL85"
          />
          <h3 className="mt-4 text-xl font-bold">Sophia Carter</h3>
          <p className="text-sm text-[#101922]/70 dark:text-[#f6f7f8]/70">@sophiacarter</p>
          <p className="mt-2 text-center text-sm text-[#101922]/90 dark:text-[#f6f7f8]/90">
            Lifestyle blogger sharing my journey and favorite finds.
          </p>
          <div className="mt-4 flex gap-4">
            <a className="hover:text-primary text-[#101922]/70 dark:text-[#f6f7f8]/70" href="#">
              <svg className="h-6 w-6" fill="currentColor" viewBox="0 0 24 24">
                <path d="M22.46 6c-.77.34-1.6.57-2.46.67.88-.53 1.56-1.37 1.88-2.38-.83.49-1.74.85-2.7 1.03A4.37 4.37 0 0 0 16.5 4c-2.38 0-4.31 1.93-4.31 4.31 0 .34.04.67.11 1-3.58-.18-6.75-1.9-8.88-4.5a4.31 4.31 0 0 0-.58 2.17c0 1.5.76 2.82 1.92 3.6a4.26 4.26 0 0 1-1.95-.54v.05c0 2.09 1.49 3.84 3.45 4.23-.36.1-.74.15-1.13.15-.28 0-.55-.03-.81-.08.55 1.71 2.14 2.96 4.03 3-1.48 1.16-3.35 1.85-5.38 1.85-.35 0-.69-.02-1.03-.06 1.92 1.23 4.2 1.95 6.63 1.95 7.95 0 12.3-6.59 12.3-12.3 0-.19 0-.37-.01-.56.84-.6 1.56-1.36 2.14-2.22z" />
              </svg>
            </a>
            <a className="hover:text-primary text-[#101922]/70 dark:text-[#f6f7f8]/70" href="#">
              <svg className="h-6 w-6" fill="currentColor" viewBox="0 0 24 24">
                <path d="M12 2.163c3.204 0 3.584.012 4.85.07 3.252.148 4.771 1.691 4.919 4.919.058 1.265.069 1.645.069 4.85s-.011 3.584-.069 4.85c-.149 3.225-1.664 4.771-4.919 4.919-1.266.058-1.644.07-4.85.07s-3.584-.012-4.85-.07c-3.26-.149-4.771-1.699-4.919-4.92-.058-1.265-.07-1.644-.07-4.85s.012-3.584.07-4.85c.149-3.227 1.664-4.771 4.919-4.919C8.416 2.175 8.796 2.163 12 2.163m0-2.163C8.74 0 8.333.011 7.053.072 2.695.272.273 2.69.073 7.052.012 8.333 0 8.74 0 12s.011 3.667.072 4.947c.2 4.358 2.618 6.78 6.98 6.98C8.333 23.988 8.74 24 12 24s3.667-.011 4.947-.072c4.354-.2 6.782-2.618 6.979-6.98.061-1.28.073-1.687.073-4.947s-.012-3.667-.073-4.947c-.197-4.354-2.625-6.78-6.98-6.979C15.667.012 15.26 0 12 0zm0 5.838a6.162 6.162 0 1 0 0 12.324 6.162 6.162 0 0 0 0-12.324zM12 16a4 4 0 1 1 0-8 4 4 0 0 1 0 8zm6.406-11.845a1.44 1.44 0 1 0 0 2.88 1.44 1.44 0 0 0 0-2.88z" />
              </svg>
            </a>
          </div>
        </div> */}
        {children}
      </div>
    </div>
  );
}
