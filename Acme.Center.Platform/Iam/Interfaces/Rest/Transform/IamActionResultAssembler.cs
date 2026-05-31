using Acme.Center.Platform.Shared.Application.Model;
using Acme.Center.Platform.Resources.Errors;
using Acme.Center.Platform.Iam.Domain.Model; // For IamError
using Acme.Center.Platform.Shared.Interfaces.Rest.ProblemDetails; // For ProblemDetailsFactory
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using System;
using System.Net.Mime; // For StatusCodes

namespace Acme.Center.Platform.Iam.Interfaces.Rest.Transform
{
    public static class IamActionResultAssembler
    {
        // --- Helper for transforming IamError to StatusCode ---
        private static int ToStatusCodeFromIamError(IamError error)
        {
            return error switch
            {
                IamError.InvalidCredentials => StatusCodes.Status400BadRequest,
                IamError.UsernameAlreadyTaken => StatusCodes.Status409Conflict,
                IamError.OperationCancelled => StatusCodes.Status409Conflict,
                IamError.DatabaseError => StatusCodes.Status500InternalServerError,
                IamError.InternalServerError => StatusCodes.Status500InternalServerError,
                IamError.ExternalServiceError => StatusCodes.Status503ServiceUnavailable, // Assuming 503 for external service issues
                _ => StatusCodes.Status400BadRequest // Default
            };
        }

        // --- Specific Assembler Methods ---

        public static IActionResult ToActionResultFromSignInResult(
            ControllerBase controller,
            Result<(Iam.Domain.Model.Aggregates.User user, string token)> result,
            IStringLocalizer<ErrorMessages> errorLocalizer,
            ProblemDetailsFactory problemDetailsFactory,
            Func<(Iam.Domain.Model.Aggregates.User user, string token), IActionResult> successAction)
        {
            if (result.IsSuccess)
            {
                return successAction(result.Value!);
            }

            var statusCode = ToStatusCodeFromIamError((IamError)result.Error!);
            return problemDetailsFactory.CreateProblemDetails(controller, statusCode, result.Error, result.Message);
        }

        public static IActionResult ToActionResultFromSignUpResult(
            ControllerBase controller,
            Result result, // Non-generic Result
            IStringLocalizer<ErrorMessages> errorLocalizer,
            ProblemDetailsFactory problemDetailsFactory,
            Func<IActionResult> successAction)
        {
            if (result.IsSuccess)
            {
                return successAction();
            }

            var statusCode = ToStatusCodeFromIamError((IamError)result.Error!);
            return problemDetailsFactory.CreateProblemDetails(controller, statusCode, result.Error, result.Message);
        }

        public static IActionResult ToActionResultFromGetUserByIdResult(
            ControllerBase controller,
            Iam.Domain.Model.Aggregates.User? user, // Direct user object, not a Result
            IStringLocalizer<ErrorMessages> errorLocalizer,
            ProblemDetailsFactory problemDetailsFactory,
            Func<Iam.Domain.Model.Aggregates.User, IActionResult> successAction)
        {
            if (user is null)
            {
                return problemDetailsFactory.CreateProblemDetails(
                    controller,
                    ToStatusCodeFromIamError(IamError.UserNotFound),
                    IamError.UserNotFound,
                    errorLocalizer[nameof(IamError.UserNotFound)]
                );
            }
            return successAction(user);
        }
    }
}