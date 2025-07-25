import { createFileRoute } from '@tanstack/react-router';
import { useState, useEffect, useRef } from 'react';



export const Route = createFileRoute('/test/img')({
  component: LazyImageDemo,
})

interface LazyImageProps {
  src: string;
  placeholder: string;
  alt: string;
  className?: string;
  onClick?: () => void;
}

const LazyImage = ({ 
  src, 
  placeholder, 
  alt, 
  className = "", 
  onClick 
}: LazyImageProps) => {
  const [isLoaded, setIsLoaded] = useState(false);
  const [isInView, setIsInView] = useState(false);
  const imgRef = useRef<HTMLDivElement>(null);

  useEffect(() => {
    const observer = new IntersectionObserver(
      ([entry]) => {
        if (entry.isIntersecting) {
          setIsInView(true);
          observer.disconnect();
        }
      },
      { threshold: 0.1 }
    );

    if (imgRef.current) {
      observer.observe(imgRef.current);
    }

    return () => observer.disconnect();
  }, []);

  return (
    <div 
      ref={imgRef}
      className={`relative overflow-hidden ${className}`}
      style={{
        backgroundImage: `url(${placeholder})`,
        backgroundSize: 'cover',
        backgroundPosition: 'center',
        backgroundRepeat: 'no-repeat'
      }}
      onClick={onClick}
    >
      <div className={`absolute inset-0 bg-white transition-opacity duration-1000 ${
        isLoaded ? 'opacity-0' : 'opacity-30 animate-pulse'
      }`} />
      
      {isInView && (
        <img
          src={src}
          alt={alt}
          loading="lazy"
          onLoad={() => setIsLoaded(true)}
          className={`w-full h-full object-cover transition-opacity duration-300 ${
            isLoaded ? 'opacity-100' : 'opacity-0'
          }`}
        />
      )}
    </div>
  );
};

// Demo Component
export default function LazyImageDemo() {
  const images = [
    {
      id: 1,
      src: 'https://picsum.photos/800/600?random=1',
      placeholder: 'https://picsum.photos/20/15?random=1&blur=2',
      alt: 'Random image 1'
    },
    {
      id: 2,
      src: 'https://picsum.photos/800/600?random=2',
      placeholder: 'https://picsum.photos/20/15?random=2&blur=2',
      alt: 'Random image 2'
    },
    {
      id: 3,
      src: 'https://picsum.photos/800/600?random=3',
      placeholder: 'https://picsum.photos/20/15?random=3&blur=2',
      alt: 'Random image 3'
    },
    {
      id: 4,
      src: 'https://picsum.photos/800/600?random=4',
      placeholder: 'https://picsum.photos/20/15?random=4&blur=2',
      alt: 'Random image 4'
    },
    {
      id: 5,
      src: 'https://picsum.photos/800/600?random=5',
      placeholder: 'https://picsum.photos/20/15?random=5&blur=2',
      alt: 'Random image 5'
    },
    {
      id: 6,
      src: 'https://picsum.photos/800/600?random=6',
      placeholder: 'https://picsum.photos/20/15?random=6&blur=2',
      alt: 'Random image 6'
    },
    {
      id: 7,
      src: 'https://picsum.photos/800/600?random=7',
      placeholder: 'https://picsum.photos/20/15?random=7&blur=2',
      alt: 'Random image 7'
    },
    {
      id: 8,
      src: 'https://picsum.photos/800/600?random=8',
      placeholder: 'https://picsum.photos/20/15?random=8&blur=2',
      alt: 'Random image 8'
    },
  ];

  return (
    <div className="min-h-screen bg-gray-50 py-8">
      <div className="max-w-4xl mx-auto px-4">
        <h1 className="text-3xl font-bold text-center mb-4 text-gray-800">
          🚀 LazyImage Component Demo
        </h1>
        <p className="text-gray-600 text-center mb-8">
          Scroll down slowly to see the lazy loading in action!
        </p>

        {/* Performance tip */}
        <div className="bg-blue-50 border-l-4 border-blue-400 p-4 mb-8">
          <div className="flex">
            <div className="ml-3">
              <p className="text-sm text-blue-700">
                💡 <strong>Pro tip:</strong> Open DevTools → Network tab to see images loading only when they come into view!
              </p>
            </div>
          </div>
        </div>

        {/* Image Grid */}
        <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
          {images.map((image) => (
            <div key={image.id} className="group">
              <LazyImage
                src={image.src}
                placeholder={image.placeholder}
                alt={image.alt}
                className="aspect-video rounded-lg shadow-lg hover:shadow-xl transition-shadow cursor-pointer"
                onClick={() => alert(`Clicked on ${image.alt}`)}
              />
              <p className="text-sm text-gray-600 mt-2 text-center">
                Image {image.id} - Click to test onClick
              </p>
            </div>
          ))}
        </div>

        {/* Large image example */}
        <div className="mt-16">
          <h2 className="text-2xl font-semibold mb-4 text-gray-800">Large Image Example</h2>
          <LazyImage
            src="https://picsum.photos/1200/800?random=9"
            placeholder="https://picsum.photos/30/20?random=9&blur=2"
            alt="Large landscape image"
            className="w-full h-96 rounded-lg shadow-lg"
          />
        </div>

        {/* Comparison section */}
        <div className="mt-16 bg-white rounded-lg shadow-lg p-8">
          <h2 className="text-2xl font-semibold mb-6 text-gray-800">🆚 Comparison Test</h2>
          
          <div className="grid grid-cols-1 md:grid-cols-2 gap-8">
            <div>
              <h3 className="text-lg font-semibold mb-3 text-green-600">✅ With LazyImage</h3>
              <LazyImage
                src="https://picsum.photos/600/400?random=10"
                placeholder="https://picsum.photos/15/10?random=10&blur=2"
                alt="Lazy loaded image"
                className="w-full h-48 rounded-lg shadow"
              />
              <p className="text-sm text-gray-600 mt-2">
                Smooth loading with placeholder → full image transition
              </p>
            </div>
            
            <div>
              <h3 className="text-lg font-semibold mb-3 text-orange-600">⚡ Native loading="lazy"</h3>
              <img
                src="https://picsum.photos/600/400?random=11"
                alt="Native lazy loaded image"
                loading="lazy"
                className="w-full h-48 object-cover rounded-lg shadow"
              />
              <p className="text-sm text-gray-600 mt-2">
                Browser default - blank space then image appears
              </p>
            </div>
          </div>
        </div>

        {/* Instructions */}
        <div className="mt-12 bg-gray-100 rounded-lg p-6">
          <h3 className="text-lg font-semibold mb-3 text-gray-800">🔍 How to Test</h3>
          <ul className="list-disc list-inside text-gray-600 space-y-2">
            <li>Open browser DevTools (F12)</li>
            <li>Go to Network tab</li>
            <li>Refresh the page</li>
            <li>Scroll down slowly</li>
            <li>Watch images load only when they come into view!</li>
            <li>Compare loading behavior between LazyImage and native lazy loading</li>
          </ul>
        </div>
      </div>
    </div>
  );
}