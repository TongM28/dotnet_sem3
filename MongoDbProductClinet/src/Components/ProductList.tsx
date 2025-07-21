// import  { useEffect, useState } from 'react'
// import { useNavigate } from 'react-router-dom';
// import { Product } from '../Interface/baseInterface';
// import API from '../API/api';


// export const ProductList = () => {
//     const navigate = useNavigate();
//     const [products, setProducts] = useState<Product[]>([]);
//     const [loading, setLoading] = useState(true);



//     const fetchProducts = async () => {
//         try {
//           const { data } = await API.get('/Product');
//           setProducts(data);
//           setLoading(false);
//         } catch (error) {
//           console.error('Error fetching products:', error);
//           setLoading(false);
//         }
//       };


   
//       const handleDelete = async (id: string) => {
//         if (window.confirm('Are you sure you want to delete this product?')) {
//           try {
//             await API.delete(`/Product/${id}`);
//             // Refresh the product list after deletion
//             fetchProducts();
//           } catch (error) {
//             console.error('Error deleting product:', error);
//           }
//         }
//       };
    


//       useEffect(() => {
//         fetchProducts();
//       }, []);
    
//       if (loading) {
//         return (
//           <div className="flex justify-center items-center min-h-screen">
//             <div className="animate-spin rounded-full h-12 w-12 border-t-2 border-b-2 border-blue-500"></div>
//           </div>
//         );
//       }



//   return (
//     <div className="container mx-auto px-4 py-8">
//     <h1 className="text-3xl font-bold text-gray-800 mb-8">Our Products</h1>
//     <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
//       {products.map((product) => (
//         <div
//           key={product.id}
//           className="bg-white rounded-lg shadow-md overflow-hidden hover:shadow-lg transition-shadow duration-300"
//         >
//           <div className="p-6">
//             <h2 className="text-xl font-semibold text-gray-800 mb-2">
//               {product.name || 'Unnamed Product'}
//             </h2>
//             <p className="text-gray-600 mb-4">
//               {product.description || 'No description available'}
//             </p>
//             <div className="flex justify-between items-center">
//               <span className="text-2xl font-bold text-blue-600">
//                 ${product.price?.toFixed(2) || 'N/A'}
//               </span>
//                <button
//                     onClick={() => navigate(`/product/${product.id}`)}
//                     className="bg-blue-500 hover:bg-blue-600 text-white px-4 py-2 rounded-md"
//                   >
//                     View Details
//                   </button>

                  

//                   <button
//                     onClick={() => handleDelete(product.id)}
//                     className="bg-red-500 hover:bg-red-600 text-white px-4 py-2 rounded-md"
//                   >
//                     Delete
//                   </button>
//             </div>
//           </div>
//         </div>
//       ))}
//     </div>
//   </div>
//   )
// }


 
import { useEffect, useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { Product } from '../Interface/baseInterface';
import API from '../API/api';

export const ProductList = () => {
  const navigate = useNavigate();
  const [products, setProducts] = useState<Product[]>([]);
  const [loading, setLoading] = useState(true);
  const [search, setSearch] = useState('');
  const [sortBy, setSortBy] = useState('');
  const [sortDirection, setSortDirection] = useState('');
  const [pageSize, setPageSize] = useState(4);
  const [page, setPage] = useState(1);
  const [totalCount, setTotalCount] = useState(0);

  const fetchProducts = async () => {
    setLoading(true);
    try {
      const { data } = await API.get('/Product/filter', {
        params: {
          search,
          sortBy,
          sortDirection,
          page,
          pageSize,
        },
      });
      setProducts(data.items);
      setTotalCount(data.totalCount);
    } catch (error) {
      console.error('Error fetching products:', error);
    } finally {
      setLoading(false);
    }
  };

  const handleDelete = async (id: string) => {
    if (window.confirm('Are you sure you want to delete this product?')) {
      try {
        await API.delete(`/Product/${id}`);
        fetchProducts();
      } catch (error) {
        console.error('Error deleting product:', error);
      }
    }
  };

  useEffect(() => {
    fetchProducts();
  }, [search, sortBy, sortDirection, page, pageSize]);

  const totalPages = Math.ceil(totalCount / pageSize);

  return (
    <div className="container mx-auto px-4 py-8">
      <h1 className="text-3xl font-bold text-gray-800 mb-4">Our Products</h1>

      {/* Filters */}
      <div className="flex flex-wrap gap-4 mb-6">
        <input
          type="text"
          placeholder="Search by name or description..."
          value={search}
          onChange={(e) => setSearch(e.target.value)}
          className="px-3 py-2 border rounded-md w-full md:w-64"
        />

        <select
          onChange={(e) => {
            const [field, direction] = e.target.value.split(',');
            setSortBy(field);
            setSortDirection(direction);
          }}
          className="px-3 py-2 border rounded-md"
        >
          <option value="">Sort</option>
          <option value="name,asc">Name (A-Z)</option>
          <option value="name,desc">Name (Z-A)</option>
          <option value="price,asc">Price (Low to High)</option>
          <option value="price,desc">Price (High to Low)</option>
        </select>

        <select
          value={pageSize}
          onChange={(e) => setPageSize(Number(e.target.value))}
          className="px-3 py-2 border rounded-md"
        >
          <option value={2}>2 per page</option>
          <option value={4}>4 per page</option>
          <option value={6}>6 per page</option>
        </select>
      </div>

      {/* Product Grid */}
      {loading ? (
        <div className="flex justify-center items-center min-h-[200px]">
          <div className="animate-spin rounded-full h-12 w-12 border-t-2 border-b-2 border-blue-500"></div>
        </div>
      ) : (
        <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
          {products.map((product) => (
            <div
              key={product.id}
              className="bg-white rounded-lg shadow-md overflow-hidden hover:shadow-lg transition-shadow duration-300"
            >
              <div className="p-6">
                <h2 className="text-xl font-semibold text-gray-800 mb-2">
                  {product.name || 'Unnamed Product'}
                </h2>
                <p className="text-gray-600 mb-4">
                  {product.description || 'No description available'}
                </p>
                <div className="flex justify-between items-center">
                  <span className="text-2xl font-bold text-blue-600">
                    ${product.price?.toFixed(2) || 'N/A'}
                  </span>
                  <div className="flex gap-2">
                    <button
                      onClick={() => navigate(`/product/${product.id}`)}
                      className="bg-blue-500 hover:bg-blue-600 text-white px-3 py-1 rounded-md"
                    >
                      View
                    </button>
                    <button
                      onClick={() => handleDelete(product.id)}
                      className="bg-red-500 hover:bg-red-600 text-white px-3 py-1 rounded-md"
                    >
                      Delete
                    </button>
                  </div>
                </div>
              </div>
            </div>
          ))}
        </div>
      )}

      {/* Pagination */}
      <div className="flex justify-center items-center mt-6 gap-4">
        <button
          disabled={page === 1}
          onClick={() => setPage(page - 1)}
          className="px-3 py-1 bg-gray-300 hover:bg-gray-400 rounded disabled:opacity-50"
        >
          Previous
        </button>
        <span>
          Page {page} of {totalPages}
        </span>
        <button
          disabled={page === totalPages}
          onClick={() => setPage(page + 1)}
          className="px-3 py-1 bg-gray-300 hover:bg-gray-400 rounded disabled:opacity-50"
        >
          Next
        </button>
      </div>
    </div>
  );
};
