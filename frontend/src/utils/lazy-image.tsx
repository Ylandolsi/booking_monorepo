import { useState, useEffect, useRef } from 'react';

interface LazyImageProps {
  src: string;
  placeholder: string;
  alt: string;
  className?: string | null;
  onClick?: () => void;
}
export const LazyImage = ({
  src,
  placeholder,
  alt,
  className = '',
  onClick,
}: LazyImageProps) => {
  const [isLoaded, setIsLoaded] = useState(false);
  const [isInView, setIsInView] = useState(false);
  const imgRef = useRef<HTMLDivElement>(null);

  useEffect(() => {
    setIsLoaded(false); // Reset the loaded state when `src` changes
  }, [src]);

  useEffect(() => {
    const observer = new IntersectionObserver(
      ([entry]) => {
        if (entry.isIntersecting) {
          setIsInView(true);
          observer.disconnect();
        }
      },
      { threshold: 0.1 },
    );

    if (imgRef.current) {
      observer.observe(imgRef.current);
    }

    return () => observer.disconnect();
  }, []);

  const handleLoad = () => {
    setIsLoaded(true);
  };

  return (
    <div
      ref={imgRef}
      className={`relative overflow-hidden ${className}`}
      style={{
        backgroundImage: `url(${placeholder})`,
        backgroundSize: 'cover',
        backgroundPosition: 'center',
        backgroundRepeat: 'no-repeat',
      }}
      onClick={onClick}
    >
      {/* Pulsing animation overlay */}
      <div
        className={`absolute inset-0 bg-white transition-opacity duration-1000 ${
          isLoaded ? 'opacity-0' : 'opacity-30 animate-pulse'
        }`}
      />

      {/* Main image */}
      {isInView && (
        <img
          src={src}
          alt={alt}
          loading="lazy"
          onLoad={handleLoad}
          className={`w-full h-full object-cover transition-opacity duration-300 ${
            isLoaded ? 'opacity-100' : 'opacity-0'
          }`}
        />
      )}
    </div>
  );
};

// Demo
// const LazyImageDemo = () => {
//   const images = [
//     {
//       id: 1,
//       src: 'https://picsum.photos/800/600?random=1',
//       placeholder: 'https://picsum.photos/20/15?random=1',
//       alt: 'Landscape 1'
//     },
//     {
//       id: 2,
//       src: 'https://picsum.photos/800/600?random=2',
//       placeholder: 'https://picsum.photos/20/15?random=2',
//       alt: 'Landscape 2'
//     },
//     {
//       id: 3,
//       src: 'https://picsum.photos/800/600?random=3',
//       placeholder: 'https://picsum.photos/20/15?random=3',
//       alt: 'Landscape 3'
//     },
//     {
//       id: 4,
//       src: 'https://picsum.photos/800/600?random=4',
//       placeholder: 'https://picsum.photos/20/15?random=4',
//       alt: 'Landscape 4'
//     },
//     {
//       id: 5,
//       src: 'https://picsum.photos/800/600?random=5',
//       placeholder: 'https://picsum.photos/20/15?random=5',
//       alt: 'Landscape 5'
//     },
//     {
//       id: 6,
//       src: 'https://picsum.photos/800/600?random=6',
//       placeholder: 'https://picsum.photos/20/15?random=6',
//       alt: 'Landscape 6'
//     }
//   ];

//   return (
//     <div className="min-h-screen bg-gray-50 py-8">
//       <div className="max-w-4xl mx-auto px-4">
//         <h1 className="text-3xl font-bold text-center mb-8">
//           Advanced Lazy Loading Images
//         </h1>
//         <p className="text-gray-600 text-center mb-8">
//           Scroll down to see the lazy loading effect with blurred placeholders
//         </p>

//         {/* Grid layout */}
//         <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
//           {images.map((image) => (
//             <LazyImage
//               key={image.id}
//               src={image.src}
//               placeholder={image.placeholder}
//               alt={image.alt}
//               className="aspect-video rounded-lg shadow-lg"
//             />
//           ))}
//         </div>

//         {/* Single large image example */}
//         <div className="mt-12">
//           <h2 className="text-2xl font-semibold mb-4">Large Image Example</h2>
//           <LazyImage
//             src="https://picsum.photos/1200/800?random=7"
//             placeholder="https://picsum.photos/30/20?random=7"
//             alt="Large landscape"
//             className="w-full h-96 rounded-lg shadow-lg"
//           />
//         </div>

//         {/* Card layout example */}
//         <div className="mt-12">
//           <h2 className="text-2xl font-semibold mb-4">Card Layout Example</h2>
//           <div className="grid grid-cols-1 md:grid-cols-2 gap-6">
//             {images.slice(0, 4).map((image) => (
//               <div key={`card-${image.id}`} className="bg-white rounded-lg shadow-lg overflow-hidden">
//                 <LazyImage
//                   src={image.src}
//                   placeholder={image.placeholder}
//                   alt={image.alt}
//                   className="w-full h-48"
//                 />
//                 <div className="p-4">
//                   <h3 className="font-semibold text-lg mb-2">{image.alt}</h3>
//                   <p className="text-gray-600">
//                     This image uses advanced lazy loading with a blurred placeholder
//                     that smoothly transitions to the full image once loaded.
//                   </p>
//                 </div>
//               </div>
//             ))}
//           </div>
//         </div>
//       </div>
//     </div>
//   );
// };

// export default LazyImageDemo;
