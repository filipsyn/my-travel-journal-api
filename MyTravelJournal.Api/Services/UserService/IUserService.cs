using ErrorOr;
using Microsoft.AspNetCore.JsonPatch;
using MyTravelJournal.Api.Contracts.V1.Requests;
using MyTravelJournal.Api.Contracts.V1.Responses;

namespace MyTravelJournal.Api.Services.UserService;

public interface IUserService
{
    /// <summary>
    /// Asynchronously retrieves data about specific user.
    /// </summary>
    /// <param name="id">A unique ID of searched user, which is to be retrieved</param>
    /// <returns>
    /// <para>
    /// A standardized response body <see cref="ServiceResponse{T}"/> carrying data with type of <see cref="UserDetailsResponse"/>.
    /// </para>
    /// </returns>
    /// 
    /// <remarks>
    /// This method returns multiple variants of <see cref="ServiceResponse{T}"/> with different contents.
    /// <list type="table">
    ///     <listheader>
    ///         <term>Status</term>
    ///         <description>Response payload</description>
    ///     </listheader>
    ///     <item>
    ///         <term><c>200</c></term>
    ///         <description>Returns <c>data</c> payload, <c>OK</c> status and <c>Success</c> set to <c>true</c></description>
    ///     </item>
    /// 
    ///     <item>
    ///         <term><c>404</c></term>
    ///         <description>Returns <c>NOT FOUND</c> status and <c>Success</c> set to <c>false</c></description>
    ///     </item>
    /// </list>
    /// </remarks>
    public Task<ErrorOr<UserDetailsResponse>> GetByIdAsync(int id);

    /// <summary>
    /// Asynchronously retrieves list of all users.
    /// </summary>
    /// <returns>
    /// <para>
    /// A standardized response body <see cref="ServiceResponse{T}"/> carrying data with type of
    /// <see cref="IEnumerable{T}"/> containing <see cref="UserDetailsResponse"/>.
    /// </para>
    /// </returns>
    /// 
    /// <remarks>
    /// This method returns multiple variants of <see cref="ServiceResponse{T}"/> with different contents.
    /// <list type="table">
    ///     <listheader>
    ///         <term>Status</term>
    ///         <description>Response payload</description>
    ///     </listheader>
    ///     <item>
    ///         <term><c>200</c></term>
    ///         <description>Returns <c>data</c> payload, <c>OK</c> status code and <c>Success</c> set to <c>true</c></description>
    ///     </item>
    /// </list>
    /// </remarks>
    public Task<IEnumerable<UserDetailsResponse>> GetAllAsync();


    /// <summary>
    /// Asynchronously retrieves data about specific user.
    /// </summary>
    /// <param name="username">An username of searched user.</param>
    /// <returns>
    /// <para>
    /// A standardized response body <see cref="ServiceResponse{T}"/> carrying data with type of <see cref="UserDetailsResponse"/>.
    /// </para>
    /// </returns>
    /// 
    /// <remarks>
    /// This method returns multiple variants of <see cref="ServiceResponse{T}"/> with different contents.
    /// <list type="table">
    ///     <listheader>
    ///         <term>Status</term>
    ///         <description>Response payload</description>
    ///     </listheader>
    ///     <item>
    ///         <term><c>200</c></term>
    ///         <description>Returns <c>data</c> payload, <c>OK</c> status and <c>Success</c> set to <c>true</c></description>
    ///     </item>
    /// 
    ///     <item>
    ///         <term><c>404</c></term>
    ///         <description>Returns <c>NOT FOUND</c> status and <c>Success</c> set to <c>false</c></description>
    ///     </item>
    /// </list>
    /// </remarks>
    public Task<ErrorOr<UserDetailsResponse>> GetByUsernameAsync(string username);


    /// <summary>
    /// Asynchronously creates new user, using information passed in a parameter. 
    /// </summary>
    /// <param name="request">A request body with information about new user</param>
    /// <returns>
    /// <para>
    /// A standardized response body <see cref="ServiceResponse{T}"/> carrying data with type of <see cref="UserDetailsResponse"/>.
    /// </para>
    /// </returns>
    /// 
    /// <remarks>
    /// This method returns multiple variants of <see cref="ServiceResponse{T}"/> with different contents.
    /// <list type="table">
    ///     <listheader>
    ///         <term>Status</term>
    ///         <description>Response payload</description>
    ///     </listheader>
    ///     <item>
    ///         <term><c>200</c></term>
    ///         <description>Returns <c>OK</c> status and <c>Success</c> set to <c>true</c></description>
    ///     </item>
    /// 
    ///     <item>
    ///         <term><c>409</c></term>
    ///         <description>Returns <c>CONFLICT</c> status and <c>Success</c> set to <c>false</c></description>
    ///     </item>
    /// </list>
    /// </remarks>
    public Task<ErrorOr<Created>> CreateAsync(CreateUserRequest request);


    /// <summary>
    /// Asynchronously performs partial update of specific user's information. 
    /// </summary>
    /// <param name="patchRequest">A request containing info about update</param>
    /// <param name="id">An unique ID of user which is updated</param>
    /// <returns>
    /// <para>
    /// A standardized response body <see cref="ServiceResponse{T}"/> carrying data with type of <see cref="UserDetailsResponse"/>.
    /// </para>
    /// </returns>
    /// 
    /// <remarks>
    /// This method returns multiple variants of <see cref="ServiceResponse{T}"/> with different contents.
    /// <list type="table">
    ///     <listheader>
    ///         <term>Status</term>
    ///         <description>Response payload</description>
    ///     </listheader>
    ///     <item>
    ///         <term><c>200</c></term>
    ///         <description>Returns <c>OK</c> status and <c>Success</c> set to <c>true</c></description>
    ///     </item>
    /// 
    ///     <item>
    ///         <term><c>404</c></term>
    ///         <description>Returns <c>NOT FOUND</c> status and <c>Success</c> set to <c>false</c></description>
    ///     </item>
    /// 
    ///     <item>
    ///         <term><c>409</c></term>
    ///         <description>Returns <c>CONFLICT</c> status and <c>Success</c> set to <c>false</c></description>
    ///     </item>
    /// 
    ///     <item>
    ///         <term><c>500</c></term>
    ///         <description>Returns <c>INTERNAL SERVER ERROR</c> status and <c>Success</c> set to <c>false</c></description>
    ///     </item>
    /// </list>
    /// </remarks>
    public Task<ServiceResponse<UserDetailsResponse>> UpdateAsync(
        JsonPatchDocument<UpdateUserDetailsRequest> patchRequest, int id);


    /// <summary>
    /// Asynchronously deletes specific user.
    /// </summary>
    /// <param name="id">An unique ID of user which is deleted</param>
    /// <returns>
    /// <para>
    /// A standardized response body <see cref="ServiceResponse{T}"/> carrying data with type of <see cref="UserDetailsResponse"/>.
    /// </para>
    /// </returns>
    /// 
    /// <remarks>
    /// This method returns multiple variants of <see cref="ServiceResponse{T}"/> with different contents.
    /// <list type="table">
    ///     <listheader>
    ///         <term>Status</term>
    ///         <description>Response payload</description>
    ///     </listheader>
    ///     <item>
    ///         <term><c>200</c></term>
    ///         <description>Returns <c>OK</c> status and <c>Success</c> set to <c>true</c></description>
    ///     </item>
    /// 
    ///     <item>
    ///         <term><c>404</c></term>
    ///         <description>Returns <c>NOT FOUND</c> status and <c>Success</c> set to <c>false</c></description>
    ///     </item>
    /// 
    ///     <item>
    ///         <term><c>409</c></term>
    ///         <description>Returns <c>CONFLICT</c> status and <c>Success</c> set to <c>false</c></description>
    ///     </item>
    /// </list>
    /// </remarks>
    public Task<ErrorOr<Deleted>> DeleteByIdAsync(int id);

    public Task<ServiceResponse<IEnumerable<TripDetailsResponse>>> GetTripsForUser(int id);
}